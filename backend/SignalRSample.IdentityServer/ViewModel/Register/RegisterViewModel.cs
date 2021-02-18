using System.ComponentModel.DataAnnotations;

namespace SignalRSample.IdentityServer.ViewModel.Register
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "USERNAME_REQUIRED")]
        public string UserName { get; set; }
        
        [Required(ErrorMessage = "EMAIL_REQUIRED")]
        [EmailAddress(ErrorMessage = "EMAIL_INVALID")]
        public string Email { get; set; }

        [Required(ErrorMessage = "PASSWORD_REQUIRED")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "CONFIRM_PASSWORD_NOT_MATCHING")]
        public string ConfirmPassword { get; set; }
    }
}