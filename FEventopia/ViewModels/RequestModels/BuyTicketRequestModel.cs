﻿using System.ComponentModel.DataAnnotations;

namespace FEventopia.Controllers.ViewModels.RequestModels
{
    public class TicketRequestModel
    {
        public required Guid EventDetailId { get; set; }

        [Range(1, 3, ErrorMessage = "You can only request maximum of 3 tickets per event per transaction!")]
        public int Quantity { get; set; }
    }

    public class BuyTicketRequestModel
    {
        public required List<TicketRequestModel> TicketRequests { get; set; }

        [DataType(DataType.EmailAddress,
                  ErrorMessage = "Email Address is not valid!")]
        [EmailAddress(ErrorMessage = "Email Address is not valid!")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$",
                  ErrorMessage = "Email Address is not valid!")]
        [Required(AllowEmptyStrings = false,
                  ErrorMessage = "Email Address required!")]
        [Display(Name = "Email Address")]
        public required string EmailAddress { get; set; }
        [Range(0, double.MaxValue)]
        public required double TotalPrice { get; set; }
    }
}
