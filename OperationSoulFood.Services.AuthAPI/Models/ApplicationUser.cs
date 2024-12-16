using Microsoft.AspNetCore.Identity;

namespace OperationSoulFood.Services.AuthAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
