using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VideogamesPOS.Data;
using VideogamesPOS.Models;
using VideogamesPOS.Models.ViewModels;

namespace VideogamesPOS.Controllers
{
    public class VideogamesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public VideogamesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: Videogames
        public async Task<IActionResult> Index(string? searchTerm, string? sortOrder,int pageNumber = 1, int pageSize = 5)
        {
            var query = _context.Videogames.AsQueryable();

            // Filtro Name
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(v => v.Name.Contains(searchTerm));
            }

            // Order
            query = sortOrder switch
            {
                "rating" => query.OrderByDescending(v => v.Rating),
                "price" => query.OrderBy(v => v.Price),
                "release" => query.OrderByDescending(v => v.ReleaseDate),
                _ => query.OrderBy(v => v.Name)
            };

            var viewModel = new VideogameIndexViewModel
            {
                SearchTerm = searchTerm,
                SortOrder = sortOrder,
                Videogames = await query.ToListAsync()
            };

            return View(viewModel);
        }


        // GET: Videogames/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videogame = await _context.Videogames
                .Include(v => v.Genres)
                .Include(v => v.Platforms)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (videogame == null)
            {
                return NotFound();
            }

            return View(videogame);
        }

        // GET: Videogames/Create
        public IActionResult Create()
        {
            var viewModel = new VideogameFormViewModel
            {
                Videogame = new Videogame(),
                AvailablePlatforms = _context.Platforms
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToList(),
                AvailableGenres = _context.Genres
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToList()

            };
            return View(viewModel);
        }

        // POST: Videogames/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VideogameFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    if (viewModel.SelectedPlatformIds == null || !viewModel.SelectedPlatformIds.Any())
                    {
                        ModelState.AddModelError("SelectedPlatformIds", "You must select at least one platform.");
                    }

                    if (viewModel.SelectedGenreIds == null || !viewModel.SelectedGenreIds.Any())
                    {
                        ModelState.AddModelError("SelectedGenreIds", "You must select at least one genre.");
                    }



                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Error en {entry.Key}: {error.ErrorMessage}");
                    }
                }
            }

            else
            {
                // Cargar las plataformas y géneros seleccionados desde la base de datos
                viewModel.Videogame.Platforms = await _context.Platforms
              .Where(p => viewModel.SelectedPlatformIds.Contains(p.Id))
              .ToListAsync();

                viewModel.Videogame.Genres = await _context.Genres
                    .Where(g => viewModel.SelectedGenreIds.Contains(g.Id))
                    .ToListAsync();

                _context.Videogames.Add(viewModel.Videogame);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Repoblar los dropdowns si hay error
            viewModel.AvailablePlatforms = _context.Platforms
               .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
               .ToList();
            viewModel.AvailableGenres = _context.Genres
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToList();

            return View(viewModel);
        }

        // GET: Videogames/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var videogame = await _context.Videogames
                .Include(v => v.Platforms)
                .Include(v => v.Genres)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (videogame == null) return NotFound();

            var viewModel = new VideogameFormViewModel
            {
                Videogame = videogame,
                SelectedPlatformIds = videogame.Platforms.Select(p => p.Id).ToList(),
                SelectedGenreIds = videogame.Genres.Select(g => g.Id).ToList(),
                AvailablePlatforms = _context.Platforms
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    }).ToList(),
                AvailableGenres = _context.Genres
                    .Select(g => new SelectListItem
                    {
                        Value = g.Id.ToString(),
                        Text = g.Name
                    }).ToList()
            };

            return View(viewModel);
        }


        // POST: Videogames/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VideogameFormViewModel viewModel)
        {
            if (id != viewModel.Videogame.Id)
                return NotFound();

            if (!ModelState.IsValid ||
                viewModel.SelectedPlatformIds == null || !viewModel.SelectedPlatformIds.Any() ||
                viewModel.SelectedGenreIds == null || !viewModel.SelectedGenreIds.Any())
            {
                if (viewModel.SelectedPlatformIds == null || !viewModel.SelectedPlatformIds.Any())
                {
                    ModelState.AddModelError("SelectedPlatformIds", "You must select at least one platform.");
                }

                if (viewModel.SelectedGenreIds == null || !viewModel.SelectedGenreIds.Any())
                {
                    ModelState.AddModelError("SelectedGenreIds", "You must select at least one genre.");
                }

                // Repoblar los dropdowns
                viewModel.AvailablePlatforms = _context.Platforms
                    .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
                    .ToList();

                viewModel.AvailableGenres = _context.Genres
                    .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                    .ToList();

                return View(viewModel);
            }

            var videogameToUpdate = await _context.Videogames
                .Include(v => v.Platforms)
                .Include(v => v.Genres)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (videogameToUpdate == null) return NotFound();

            // Actualizar propiedades simples
            videogameToUpdate.Name = viewModel.Videogame.Name;
            videogameToUpdate.Description = viewModel.Videogame.Description;
            videogameToUpdate.ReleaseDate = viewModel.Videogame.ReleaseDate;
            videogameToUpdate.Price = viewModel.Videogame.Price;
            videogameToUpdate.ImageUrl = viewModel.Videogame.ImageUrl;
            // etc.

            // Actualizar relaciones muchos a muchos
            videogameToUpdate.Platforms = await _context.Platforms
                .Where(p => viewModel.SelectedPlatformIds.Contains(p.Id))
                .ToListAsync();

            videogameToUpdate.Genres = await _context.Genres
                .Where(g => viewModel.SelectedGenreIds.Contains(g.Id))
                .ToListAsync();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Videogames/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var videogame = await _context.Videogames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (videogame == null)
            {
                return NotFound();
            }

            return View(videogame);
        }

        // POST: Videogames/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var videogame = await _context.Videogames.FindAsync(id);
            if (videogame != null)
            {
                _context.Videogames.Remove(videogame);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VideogameExists(int id)
        {
            return _context.Videogames.Any(e => e.Id == id);
        }

        [HttpGet]
        public async Task<IActionResult> AutocompleteFromRawg(string name)
        {
            var apiKey = _configuration["Rawg:ApiKey"];
            using var client = new HttpClient();

            var url = $"https://api.rawg.io/api/games?search={Uri.EscapeDataString(name)}&key={apiKey}";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return Json(null);

            var json = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(json);

            var first = result["results"]?.FirstOrDefault();
            if (first == null)
                return Json(null);

            var gameName = first["name"]?.ToString() ?? "";
            var releaseDate = first["released"]?.ToString() ?? "";
            var imageUrl = first["background_image"]?.ToString() ?? "";

            var esrbSlug = "";
            var esrbToken = first["esrb_rating"];
            if (esrbToken != null && esrbToken.Type == JTokenType.Object)
            {
                esrbSlug = esrbToken["slug"]?.ToString() ?? "";
            }
            var gameId = first["id"]?.ToString();

            string description = "";
            List<string> platforms = new();
            List<string> genres = new();

            if (!string.IsNullOrEmpty(gameId))
            {
                var gameDetailsUrl = $"https://api.rawg.io/api/games/{gameId}?key={apiKey}";
                var detailsResponse = await client.GetAsync(gameDetailsUrl);
                if (detailsResponse.IsSuccessStatusCode)
                {
                    var detailJson = await detailsResponse.Content.ReadAsStringAsync();
                    var detail = JObject.Parse(detailJson);

                    description = detail["description_raw"]?.ToString() ?? "";

                    // Extraer plataformas (ej: "PC", "PlayStation 5")
                    platforms = detail["platforms"]
                        ?.Select(p => p["platform"]?["name"]?.ToString())
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Distinct()
                        .ToList() ?? new();

                    // Extraer géneros (ej: "Action", "RPG")
                    genres = detail["genres"]
                        ?.Select(g => g["name"]?.ToString())
                        .Where(g => !string.IsNullOrWhiteSpace(g))
                        .Distinct()
                        .ToList() ?? new();
                }
            }

            return Json(new
            {
                name = gameName,
                description = description,
                rating = esrbSlug,
                releaseDate = releaseDate,
                imageUrl = imageUrl,
                platforms = platforms,
                genres = genres
            });
        }

        [HttpGet]
        public async Task<IActionResult> SuggestTitlesFromRawg(string term)
        {
            var apiKey = _configuration["Rawg:ApiKey"];
            using var client = new HttpClient();

            var url = $"https://api.rawg.io/api/games?search={Uri.EscapeDataString(term)}&key={apiKey}&page_size=5";
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return Json(new List<string>());

            var json = await response.Content.ReadAsStringAsync();
            var result = JObject.Parse(json);
            var suggestions = result["results"]?
                .Select(g => g["name"]?.ToString())
                .Where(name => !string.IsNullOrEmpty(name))
                .Distinct()
                .ToList();

            return Json(suggestions);
        }



    }
}
