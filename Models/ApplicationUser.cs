using Microsoft.AspNetCore.Identity;

namespace Assignment1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string CreditCardNumber { get; set; }
        public string Gender { get; set; }
        public string MobileNo { get; set; }
        public string DeliveryAddress { get; set; }
        public string? Photo { get; set; }
        public string? Description { get; set; }
    }
}
