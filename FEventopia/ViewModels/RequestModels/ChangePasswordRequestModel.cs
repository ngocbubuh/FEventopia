using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FEventopia.Controllers.ViewModels.RequestModels
{
    public class ChangePasswordRequestModel
    {
        [Required(ErrorMessage = "Username is required!")]
        [EmailAddress(ErrorMessage = "Must be email formated!")]
        [Display(Name = "Username")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Current Password is required!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 7, ErrorMessage = "Password must be 7-30 Character")]
        [Display(Name = "Current Password")]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password is required!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 7, ErrorMessage = "Password must be 7-30 Character")]
        [Display(Name = "New Password")]
        public required string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new Password is required!")]
        [PasswordPropertyText]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 7, ErrorMessage = "Password must be 7-12 Character")]
        [Display(Name = "Confirm new Password")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation does not match!")]
        public required string ConfirmNewPassword { get; set; }
    }
}
