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
        public required Guid EventDetailID { get; set; }

        [Required(ErrorMessage = "StudentID required!")]
        public required string VisitorID { get; set; }

        [Required(ErrorMessage = "Price required!")]
        public required Guid TransactionID { get; set; }
        public bool? CheckInStatus { get; set; } = false;

        public virtual Account? Account { get; } //FK VisitorID
        public virtual Transaction? Transaction { get; } //FK TransactionID
        public virtual EventDetail? EventDetail { get; }
    }
}
