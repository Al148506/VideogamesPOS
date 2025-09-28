using VideogamesPOS.Models.ViewModels;
using VideogamesPOS.Models;
using Microsoft.EntityFrameworkCore;
using VideogamesPOS.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace VideogamesPOS.Helper
{
    public  class BuildFormVideogames
    {
        private readonly ApplicationDbContext _context;
        public BuildFormVideogames(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<VideogameFormViewModel> BuildFormViewModelAsync(Videogame videogame)
        {
            var vm = new VideogameFormViewModel
            {
                Videogame = videogame,
                SelectedPlatformIds = videogame.Platforms.Select(p => p.Id).ToList(),
                SelectedGenreIds = videogame.Genres.Select(g => g.Id).ToList()
            };

            await PopulateSelectListsAsync(vm);
            return vm;
        }
        public async Task PopulateSelectListsAsync(VideogameFormViewModel vm)
        {
            // AsNoTracking is fine for read-only lists
            vm.AvailablePlatforms = await _context.Platforms
                .AsNoTracking()
                .Select(p => new SelectListItem { Value = p.Id.ToString(), Text = p.Name })
            .ToListAsync();

            vm.AvailableGenres = await _context.Genres
                .AsNoTracking()
                .Select(g => new SelectListItem { Value = g.Id.ToString(), Text = g.Name })
                .ToListAsync();
        }
    }
}
