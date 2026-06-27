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
    public class CategoryController : ControllerBase
    {
        private readonly LibraryManagementContext _context;
        private readonly IMapper _mapper;
        public CategoryController(LibraryManagementContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           var categories = await _context.Categories.Include(x=>x.Books).ToListAsync();
            var result = _mapper.Map<List<CategoryListDto>>(categories);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var findingCategory = await _context.Categories.Include(x=>x.Books).FirstOrDefaultAsync(x=>x.Id == id);
            if(findingCategory == null)
            {
                return NotFound("Bu categoriya movcud deyil");
            }
            var result = _mapper.Map<CategoryListDto>(findingCategory);
            return Ok(result);  
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto category)
        {
            if (string.IsNullOrEmpty(category.Name))
            {
                return BadRequest("Kategoriya adi bos ola bilmez");
            }
            var categorydto = _mapper.Map<Category>(category);
            _context.Categories.Add(categorydto);   
            await _context.SaveChangesAsync();
            return Ok("Kategoriya ugurla yaradildi");
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult>UpdateCategory(int id, [FromBody] CategoryDto dto)
        {
            var existCategory = await _context.Categories.FindAsync(id);
            if (existCategory == null)
            {
                return NotFound("Bu categoriya movcud deyil");
            }
            _mapper.Map(dto, existCategory);
            _context.Categories.Update(existCategory);
            await _context.SaveChangesAsync();
            return Ok("Kategoriya ugurla yenilendi");
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult>Delete(int id)
        {
            var deletedcategory = await _context.Categories.FindAsync(id);
            if (deletedcategory == null) return NotFound("bu kategoriya movcud deyil");
            _context.Categories.Remove(deletedcategory);
            await _context.SaveChangesAsync();
            return Ok("Kategoriya ugurla silindi");
    }
    }
}