using LibraryManagment.DTOs;
using LibraryManagment.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public AuthController(UserManager<User> userManager,IConfiguration configuration)
        {
            _userManager = userManager; 
            _configuration = configuration;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new User
            {
                FullName = dto.FullName,
                UserName = dto.Username,
                Email = dto.Email,
                Role = dto.Username.ToLower().Contains("admin")? "Admin": "User"
            };
            if(user is null)
            {
                return NotFound("User tapilmadi");
            }
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
            {
                return Ok(new { message = "Istifadeci ugurla qeydiyyatdan kecdi!" });
            }
            return BadRequest(result.Errors);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            if(user is null)
            {
                return BadRequest(new { message = "Istifadeci adi ve ya sifre yanlisdir!" });
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!isPasswordValid)
            {
                return BadRequest(new { message = "Istifadeci adi ve ya sifre yanlisdir" });
            }
            var token = GenerateJwtToken(user);

            // İstifadəçiyə həm uğurlu mesajı, həm də tokeni qaytarırıq
            return Ok(new
            {
                message = "Giriş uğurludur!",
                token = token
            });

        }   
        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tokenin ömrünü 1 saat edirik
            var expires = DateTime.UtcNow.AddHours(1);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
    }
    

