using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class Task : EntityBase
    {
        public required string Description { get; set; }
        public required string StaffID { get; set; }
        public required string Status { get; set; }
        public required Guid EventDetailID { get; set; }
        public double? PlanCost { get; set; } = 0;
        public double? ActualCost { get; set; } = 0;

        public virtual Account? Account { get; } //FK StaffID
        public virtual EventDetail? EventDetail { get; }
    }
}
