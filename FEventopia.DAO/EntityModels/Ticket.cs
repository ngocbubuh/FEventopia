using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class Ticket : EntityBase
    {
        [Required(ErrorMessage = "EventDetailID required!")]
        public required string EventDetailID { get; set; }

        [Required(ErrorMessage = "StudentID required!")]
        public required string VisitorID { get; set; }

        [Required(ErrorMessage = "Price required!")]
        public required string TransactionID { get; set; }
        public bool CheckInStatus { get; set; } = false;

        public required virtual Account Account { get; set; } //FK VisitorID
        public required virtual Transaction Transaction { get; set; } //FK TransactionID
        public virtual required EventDetail EventDetail { get; set; }
    }
}
