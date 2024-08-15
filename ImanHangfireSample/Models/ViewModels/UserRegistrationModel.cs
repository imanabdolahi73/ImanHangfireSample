using System.ComponentModel.DataAnnotations;

namespace SampleHangfire.Models.ViewModels
{
    public class UserRegistrationModel
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        [Required(ErrorMessage = "PhoneNumber is required")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email{ get; set; } = string.Empty;
        [Required(ErrorMessage = "Password  is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [DataType(DataType.Password)]
        [Compare("Password" , ErrorMessage ="The password and confirmation password do not match")]
        public string ConfirmPassword { get; set; } = string.Empty;

    }
    public class UserLoginModel
    {
        [Required(ErrorMessage = "UserName is required")]
        [EmailAddress]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Password  is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}
