using System.ComponentModel.DataAnnotations;


namespace Assignment1.ViewModels
{
    public class ResetPassword
    {
        // Properties

        // New Password
        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        // Confirm New Password
        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
