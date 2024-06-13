using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
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
        public required string Role { get; set; }
        public double CreditAmount { get; set; } = 0;

        [Timestamp]
        public byte[]? Version { get; set; }
        public virtual ICollection<Transaction>? Transaction { get; }
        public virtual ICollection<Ticket>? Ticket { get; }
        public virtual ICollection<SponsorEvent>? SponsorEvents { get; }
        public virtual ICollection<EventStall>? EventStall { get; }
        public virtual ICollection<Task>? Task { get; }
        public virtual ICollection<Feedback>? Feedback { get; } 
        public virtual ICollection<SponsorManagement>? SponsorManagement { get; }
        public virtual ICollection<EventAssignee>? EventAssignee { get; }
    }
}
