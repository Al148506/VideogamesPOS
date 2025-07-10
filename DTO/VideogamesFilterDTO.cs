namespace VideogamesPOS.DTO
{
    public class VideogamesFilterDTO
    {
        public int Page { get; set; } = 1;
        public int RecordsPerPage { get; set; } = 10;

        public string? SearchTerm { get; set; }
        public string? SortOrder { get; set; }

        public PaginationDTO Pagination =>
            new PaginationDTO { PageNumber = Page, RecordsPerPage = RecordsPerPage };
    }

}
