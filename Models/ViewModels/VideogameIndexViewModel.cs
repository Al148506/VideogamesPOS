namespace VideogamesPOS.Models.ViewModels
{
    public class VideogameIndexViewModel
    {
        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }
        public string? SortDirection { get; set; }

        public int PageNumber { get; set; }
        public int RecordsPerPage { get; set; }
        public int TotalItems { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / RecordsPerPage);

        public List<Videogame> Videogames { get; set; } = new();
    }

}
