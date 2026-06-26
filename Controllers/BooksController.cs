using LibraryManagment.DTOs;
using LibraryManagment.Models.Context;
using LibraryManagment.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryManagementContext _context;
        public BooksController(LibraryManagementContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var booksDto = await _context.Books.Include(x => x.Author).Select(book => new BookResultDto
            {
                Id = book.Id,
                BookName = book.BookName,
                BookRating = book.BookRating,
                AuthorFullName = book.Author != null ? $"{book.Author.Name} {book.Author.Surname}" : "Muellif yoxdur"
            }).ToListAsync();

            return Ok(booksDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateBook(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return Ok(book);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooksById(int id)
        {
            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound("Kitab tapilmadi");
            }
            return Ok(books);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
        {
            var dbBook = await _context.Books.FindAsync(id);
            if (dbBook == null) {
                return NotFound("Yenilenecek kitab tapilmadi");
            }
            dbBook.BookName = updatedBook.BookName;
            dbBook.BookRating = updatedBook.BookRating;
            dbBook.AuthorId = updatedBook.AuthorId;
            await _context.SaveChangesAsync();
            return Ok(dbBook);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult>DeleteBooks(int id)
        {
            var deletingbooks = await _context.Books.FindAsync(id);
            if(deletingbooks == null)
            {
                return NotFound("Silinecek kitab tapilmadi");
            }
            _context.Books.Remove(deletingbooks);
            await _context.SaveChangesAsync();

            return Ok("Kitab ugurla silindi");
        }
        

    } 
}
