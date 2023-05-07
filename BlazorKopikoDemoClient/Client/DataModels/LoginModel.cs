using System.ComponentModel.DataAnnotations;

namespace BlazorKopikoDemoClient.Client.DataModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Must be at least 6 characters long.")]
        public string Password { get; set; }
    }
}
