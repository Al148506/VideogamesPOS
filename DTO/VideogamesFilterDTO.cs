namespace VideogamesPOS.DTO
{
    public class VideogamesFilterDTO
    {
        public int PageNumber { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 3;


        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }
        public string? SortDirection { get; set; } = "asc"; // "asc" o "desc"


        public PaginationDTO Pagination =>
            new PaginationDTO { PageNumber = PageNumber, RecordsPerPage = RecordsPerPage };
    }

}
