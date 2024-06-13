using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class EventAssignee : EntityBase
    {
        public required string AccountId { get; set; }
        public required Guid EventDetailId { get; set; }
        public required string Role { get; set; }

        public Account? Account { get; }
        public EventDetail? EventDetail { get; }
    }
}
