using Microsoft.AspNetCore.Identity;

namespace Assignment1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string CreditCardNumber { get; set; }
    }
}
