using System.ComponentModel.DataAnnotations;


namespace Assignment1.ViewModels
{
    public class Login
    {
        // Properties
        // EmailAddress
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
        // Password
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
