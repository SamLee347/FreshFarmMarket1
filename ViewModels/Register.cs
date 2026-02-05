using System.ComponentModel.DataAnnotations;


namespace Assignment1.ViewModels
{
    public class Register : IValidatableObject
    {
        // Properties
        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Full Name can only contain letters and spaces.")]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        [RegularExpression(@"^\d{13,19}$", ErrorMessage = "Credit Card Number must be between 13 and 19 digits.")]
        public string CreditCardNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Gender can only contain letters and spaces.")]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [RegularExpression(@"^[a-zA-Z0-9\s#,.-]+$", ErrorMessage = "Delivery Address contains invalid characters.")]
        public string DeliveryAddress { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? Photo { get; set; }

        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Photo != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp", "image/webp" };
                if (!allowedTypes.Contains(Photo.ContentType))
                {
                    yield return new ValidationResult(
                        "Only image files (jpg, png, gif, bmp, webp) are allowed.",
                        new[] { nameof(Photo) });
                }
            }
        }
    }
}
