using FEventopia.Repositories.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.EntityModels
{
    public class Ticket : EntityBase
    {
        [Required(ErrorMessage = "EventDetailID required!")]
        public required string EventDetailID { get; set; }

        [Required(ErrorMessage = "StudentID required!")]
        public required string VisitorId { get; set; }

        [Required(ErrorMessage = "Price required!")]
        public DateTime PaymentDate { get; set; }

        public bool CheckInStatus { get; set; } = false;
    }
}
