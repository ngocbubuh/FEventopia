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
        public required string Banner { get; set; }
        public required string OperatorID { get; set; }
        public required string Status { get; set; }
        public required virtual Account Account { get; set; } //FK OperatorID
        public virtual SponsorEvent? SponsorEvent { get; }
        public virtual ICollection<EventDetail>? EventDetail { get; }
    }
}
