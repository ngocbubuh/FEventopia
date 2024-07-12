using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.BussinessModels
{
    public class EventDetailProcessModel
    {
        public required Guid EventID { get; set; }
        public required Guid LocationID { get; set; }
        [Required(ErrorMessage = "Start date-time is required!")]
        public required DateTime StartDate { get; set; }
        [Required(ErrorMessage = "End date-time is required!")]
        public required DateTime EndDate { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Ticket for sale must >= 1!")]
        public required int TicketForSaleInventory { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Stall for sale must >= 0!")]
        public int? StallForSaleInventory { get; set; } = 0;
        [Range(0, double.MaxValue, ErrorMessage = "Price must be minimun 0")]
        public double? StallPrice { get; set; } = 0;
        [Range(0, double.MaxValue, ErrorMessage = "Price must be minimun 0")]
        public double? TicketPrice { get; set; } = 0;

        //StartDate and EndDate duration cannot over 24 hour
        public bool IsValidDate()
        {
            TimeSpan timeDifference = StartDate - EndDate;
            return timeDifference.TotalHours <= 0;
        }
    }
}
