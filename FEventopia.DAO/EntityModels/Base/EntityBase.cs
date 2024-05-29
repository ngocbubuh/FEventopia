using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.EntityModels.Base
{
    public class EntityBase
    {
        [Column(TypeName = "varchar(15)")]
        public required string Id { get; set; }
        [Column(TypeName = "date")]
        public required DateTime CreatedDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public required string CreatedBy { get; set; }
        [Column(TypeName = "date")]
        public required DateTime UpdatedDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public required string UpdatedBy { get; set; }
        public required bool DeleteFlag { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
