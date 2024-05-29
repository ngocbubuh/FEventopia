using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class Cost : EntityBase
    {
        public required string EventDetailID { get; set; }
        public required double CostAmount { get; set; }
        public required double CostPurpose { get; set; }

        public virtual required EventDetail EventDetail { get; set; }
    }
}
