using AutoMapper;
using LibraryManagment.Context;
using LibraryManagment.DTOs;
using LibraryManagment.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly LibraryManagementContext _context;
        private readonly IMapper _mapper;
        public AuthorsController(LibraryManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            /* var author = await _context.Authors.Include(x=>x.Books).Select(authordb=> new AuthorDto
             {
                 Id = authordb.Id,
                 Name = authordb.Name,
                 Surname = authordb.Surname,
                 Age = authordb.Age,
                 BookNames = authordb.Books.Select(b=>b.BookName).ToList()
             }).ToListAsync();
             return Ok(author);*/
            var authors = await _context.Authors.Include(x => x.Books).ToListAsync();
            var result = _mapper.Map<List<AuthorDto>>(authors);
            return Ok(result);

        
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetbyId(int id)
        {
            var author = await _context.Authors.FindAsync(id);
            if (author is null)
            {
                return NotFound("Muellif tapilmadi");
            }
            return Ok(author);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAuthor(Author author)
        {
            await _context.Authors.AddAsync(author);
            await _context.SaveChangesAsync();
            return Ok(author);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(Author author, int id)
        {
            var updatedAuthor = await _context.Authors.FindAsync(id);
            if (updatedAuthor is null)
            {
                return NotFound("Muellif tapilmadi");
            }
            updatedAuthor.Name = author.Name;
            updatedAuthor.Surname = author.Surname;
            updatedAuthor.Age = author.Age;
            await _context.SaveChangesAsync();
            return Ok(updatedAuthor);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>DeleteAuthor(int id)
        {
            var deletedAuthor = await _context.Authors.FindAsync(id);
            if(deletedAuthor is null)
            {
                return NotFound("Muellif tapilmadi");
            }
            _context.Authors.Remove(deletedAuthor);
            await _context.SaveChangesAsync();
            return Ok("Muellif ugurla silindi");
        }

}
}
