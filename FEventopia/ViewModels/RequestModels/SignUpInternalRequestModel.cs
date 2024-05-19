using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FEventopia.Controllers.ViewModels.RequestModels
{
    public class SignUpInternalRequestModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [Display(Name = "Name")]
        public required string Name { get; set; } //User display name

        [Required(ErrorMessage = "Name is required!")]
        [Display(Name = "Name")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Username must be 4-15 Characters")]
        public required string Username { get; set; } //Username

        [Required(ErrorMessage = "Password is required!")]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(30, MinimumLength = 7, ErrorMessage = "Password must be 7-30 Characters")]
        [PasswordPropertyText]
        public required string Password { get; set; } //Password

        [Required(ErrorMessage = "Confirm Password is required!")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation does not match!")]
        [StringLength(30, MinimumLength = 7, ErrorMessage = "Password must be 7-30 Characters")]
        [PasswordPropertyText]
        public required string ConfirmPassword { get; set; } //Confirm Password
    }
}
