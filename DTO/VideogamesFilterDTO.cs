namespace VideogamesPOS.DTO
{
    public class VideogamesFilterDTO
    {
        public int Page { get; set; }
        public int RecordsPerPage { get; set; }
        internal PaginationDTO Pagination
        {
            get
            {
                return new PaginationDTO { Page = Page, RecordsPerPage = RecordsPerPage };
            }
        }

        public string? Name { get; set; }
        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool ComingSoon { get; set; }

    }
}
