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

        [DataType(DataType.EmailAddress,
                  ErrorMessage = "Email Address is not valid")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$",
                  ErrorMessage = "Email Address is not valid")]
        [Required(AllowEmptyStrings = false,
                  ErrorMessage = "Email Address required!")]
        [Display(Name = "Email Address")]
        public required string Email { get; set; } //Email Address
    }
}
