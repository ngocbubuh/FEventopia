using FEventopia.Repositories.EntityModels.Base;
using System.ComponentModel.DataAnnotations;

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
