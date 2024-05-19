using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FEventopia.Controllers.ViewModels.RequestModels
{
    public class SignInRequestModel
    {
        [Required(AllowEmptyStrings = false,
          ErrorMessage = "Email Address required!")]
        [Display(Name = "Email Address")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Password required!")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public required string Password { get; set; }
    }
}
