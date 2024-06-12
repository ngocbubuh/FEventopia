using FEventopia.DAO.EntityModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels
{
    public class Location : EntityBase
    {
        public required string LocationName { get; set; }
        public required string LocationDescription { get; set; }
        public required int Capacity { get; set; }
        public virtual EventDetail? EventDetail { get; }
    }
}
