using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FEventopia.Repositories.EntityModels
{
    public class Account : IdentityUser
    {
        [Required(AllowEmptyStrings = false,
                  ErrorMessage = "Display Name required!")]
        [Display(Name = "Display Name")]
        public required string Name { get; set; } //Tên người dùng

        [DataType(DataType.PhoneNumber,
                  ErrorMessage = "Phone Number is not valid!")]
        [Phone(ErrorMessage = "Phone Number is not valid!")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$",
                  ErrorMessage = "Phone Number is not valid!")]
        [Display(Name = "Phone Number")]
        public override string? PhoneNumber { get; set; } //Số điện thoại

        [DataType(DataType.EmailAddress,
                  ErrorMessage = "Email Address is not valid")]
        [EmailAddress(ErrorMessage = "Email Address is not valid")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$",
                  ErrorMessage = "Email Address is not valid")]
        [Display(Name = "Email Address")]
        public override string? Email { get; set; } //Email
        public string? Avatar { get; set; } //Link to Avatar file
        public bool DeleteFlag { get; set; } //Delete Flag

        [Timestamp]
        public byte[]? Version { get; set; }

        //Will be update soon...
    }
}
