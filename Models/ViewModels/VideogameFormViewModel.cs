

using Microsoft.AspNetCore.Mvc.Rendering;

namespace VideogamesPOS.Models.ViewModels
{
    public class VideogameFormViewModel
    {
        public VideogameFormViewModel()
        {
            SelectedPlatformIds = new List<int>();
            SelectedGenreIds = new List<int>();
            AvailablePlatforms = new List<SelectListItem>();
            AvailableGenres = new List<SelectListItem>();
        }
        public Videogame Videogame { get; set; }

        public List<SelectListItem> AvailablePlatforms { get; set; }
        public List<int> SelectedPlatformIds { get; set; }

        public List<SelectListItem> AvailableGenres { get; set; }
        public List<int> SelectedGenreIds { get; set; }

        public List<SelectListItem> AvailableRatings { get; set; } = new()
        {
            new SelectListItem { Value = "E", Text = "E (Everyone)" },
            new SelectListItem { Value = "E10+", Text = "E10+ (Everyone 10+)" },
            new SelectListItem { Value = "T", Text = "T (Teen)" },
            new SelectListItem { Value = "M", Text = "M (Mature)" },
            new SelectListItem { Value = "AO", Text = "AO (Adults Only)" },
            new SelectListItem { Value = "RP", Text = "RP (Rating Pending)" },
        };

    }
}
