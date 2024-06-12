using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class Event : EntityBase
    {
        public required string EventName { get; set; }
        public required string EventDescription { get; set; }
        public required string Category { get; set; }
        public string? Banner { get; set; }
        public required string OperatorID { get; set; }
        public required double InitialCapital { get; set; }
        public double? SponsorCapital { get; set; } = 0; //No manually update
        public double? TicketSaleIncome { get; set; } = 0; //No manually update
        public required string Status { get; set; }
        public virtual Account? Account { get; } //FK OperatorID
        public virtual SponsorEvent? SponsorEvent { get; }
        public virtual ICollection<EventDetail>? EventDetail { get; }
        public virtual ICollection<SponsorManagement>? SponsorManagement { get; }
    }
}
