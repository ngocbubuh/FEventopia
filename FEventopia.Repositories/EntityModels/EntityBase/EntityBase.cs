using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.EntityModels.EntityBase
{
    public class EntityBase
    {
        public int Id { get; set; }
        public required DateTime CreatedDate { get; set; }
        public required string CreatedBy { get; set; }
        public required DateTime UpdatedDate { get; set; }
        public required string UpdatedBy { get; set; }
        public required bool DeleteFlag { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
