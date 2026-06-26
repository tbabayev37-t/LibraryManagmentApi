using AutoMapper;
using LibraryManagment.DTOs;
using LibraryManagment.Models.Entities;

namespace LibraryManagment.Profiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookResultDto>().ForMember(dest => dest.AuthorFullName, opt => opt.MapFrom(src =>
            src.Author != null ? $"{src.Author.Name} {src.Author.Surname}" : "Muellif yoxdur"));

            CreateMap<Author, AuthorDto>().ForMember(dest => dest.BookNames, opt => opt.MapFrom(src =>
            src.Books != null ? src.Books.Select(x => x.BookName).ToList() : new List<string>()
            ));
        }
    }
}
