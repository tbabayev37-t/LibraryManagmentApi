namespace LibraryManagment.DTOs
{
    public class BookInCategoryDto
    {
        public int Id { get; set; }
        public string BookName { get; set; } = string.Empty;
        public decimal BookRating { get; set; }
    }
    public class CategoryListDto
    {
        public int Id { get; set; } 
        public string Name { get; set; }=string.Empty;
        public List<BookInCategoryDto> Books { get; set; } = new();
    }
}
