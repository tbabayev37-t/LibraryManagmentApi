namespace LibraryManagment.DTOs
{
    public class BookResultDto
    {
        public int Id { get; set; }
        public string BookName { get; set; } = null!;
        public double? BookRating { get; set; }
        public string AuthorFullName { get; set; } = null!;
    }
}
