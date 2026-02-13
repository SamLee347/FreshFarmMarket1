using System.ComponentModel.DataAnnotations;

namespace Assignment1.ViewModels
{
    public class ForgotPassword
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }
    }
}
