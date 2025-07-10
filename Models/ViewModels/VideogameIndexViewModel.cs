namespace VideogamesPOS.Models.ViewModels
{
    public class VideogameIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public List<Videogame> Videogames { get; set; } = new();
    }

}
