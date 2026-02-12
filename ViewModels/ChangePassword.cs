using System.ComponentModel.DataAnnotations;


namespace Assignment1.ViewModels
{
    public class ChangePassword : IValidatableObject
    {
        // Properties

        // Password
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // New Password
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        // Confirm New Password
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(NewPassword) && Password == NewPassword)
            {
                yield return new ValidationResult(
                    "New password must be different from the current password.",
                    new[] { nameof(NewPassword) }
                );
            }
        }
    }
}
