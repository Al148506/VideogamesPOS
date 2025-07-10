namespace VideogamesPOS.DTO
{
    public class PaginationDTO
    {
        private int recordsPerPage = 5;
        private readonly int maxRecordsPerPage = 100;

        public int PageNumber { get; set; } = 1;

        public int RecordsPerPage
        {
            get => recordsPerPage;
            set => recordsPerPage = (value > maxRecordsPerPage) ? maxRecordsPerPage : value;
        }

        public int Skip => (PageNumber - 1) * RecordsPerPage;
    }

}
