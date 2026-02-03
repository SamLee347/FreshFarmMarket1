using System.ComponentModel.DataAnnotations;


namespace Assignment1.ViewModels
{
    public class Register : IValidatableObject
    {
        // Properties
        [Required]
        [DataType(DataType.Text)]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.CreditCard)]
        public string CreditCardNumber { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string MobileNo { get; set; }

        [Required]
        [DataType(DataType.Text)]
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
