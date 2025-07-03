namespace VideogamesPOS.Models.ViewModels
{
    public class VideogameIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }

        public List<Videogame> Videogames { get; set; } = new();
    }
}
