using Microsoft.AspNetCore.Identity;

namespace LibraryManagment.Models.Entities
{
    public class User:IdentityUser
    {
        public string FullName { get; set; } = null!;
    }
}
