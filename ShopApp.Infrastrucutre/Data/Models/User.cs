using Microsoft.AspNetCore.Identity;

namespace ShopApp.Server.Data.Models
{
    public class User:IdentityUser<Guid>
    {  
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
