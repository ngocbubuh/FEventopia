using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class Feedback : EntityBase
    {
        public required Guid EventDetailId { get; set; }
        public required string AccountId { get; set; }
        public required int Rate { get; set; }
        public string? Description { get; set; }
        public virtual Account? Account { get; }
        public virtual EventDetail? EventDetail { get; }
    }
}
