using Microsoft.AspNetCore.Identity;

namespace ServicesLib.Domain.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

    }
}