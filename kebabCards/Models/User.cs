using Microsoft.AspNetCore.Identity;

namespace kebabCards.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
