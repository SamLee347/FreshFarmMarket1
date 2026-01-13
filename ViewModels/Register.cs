using System.ComponentModel.DataAnnotations;


namespace Assignment1.ViewModels
{
    public class Register
    {
        // Properties
        // FullName
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; }
        // CreditCardNumber
        [Required]
        [DataType(DataType.CreditCard)]
        public string CreditCardNumber { get; set; }
        // Gender
        [Required]
        [DataType(DataType.Text)]
        public string Gender { get; set; }
        // MobileNo
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }
        // DeliveryAddress
        [Required]
        [DataType(DataType.Text)]
        public string DeliveryAddress { get; set; }
        // EmailAddress
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        // Password
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        // ConfirmPassword
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        // Photo
        [DataType(DataType.Upload)]
        public string Photo { get; set; }
        // Description
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}
