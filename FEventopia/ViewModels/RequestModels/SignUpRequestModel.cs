using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FEventopia.Controllers.ViewModels.RequestModels
{
    public class SignUpRequestModel
    {
        [Required(ErrorMessage = "Name is required!")]
        [Display(Name = "Name")]
        public required string Name { get; set; } //User display name

        [DataType(DataType.PhoneNumber,
                  ErrorMessage = "Phone Number is not valid!")]
        [Phone(ErrorMessage = "Phone Number is not valid!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                  ErrorMessage = "Phone Number is not valid!")]
        [Required(AllowEmptyStrings = false,
                  ErrorMessage = "Phone Number required!")]
        [Display(Name = "Phone Number")]
        public required string PhoneNumber { get; set; } //Phone Number

        [DataType(DataType.EmailAddress,
                  ErrorMessage = "Email Address is not valid")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$",
                  ErrorMessage = "Email Address is not valid")]
        [Required(AllowEmptyStrings = false,
                  ErrorMessage = "Email Address required!")]
        [Display(Name = "Email Address")]
        public required string Email { get; set; } //Email Address

        [Required(ErrorMessage = "Name is required!")]
        [Display(Name = "Name")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Username must be 4-15 Characters")]
        public required string Username { get; set; } //

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
