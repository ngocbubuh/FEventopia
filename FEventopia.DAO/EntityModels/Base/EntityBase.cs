using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace FEventopia.DAO.EntityModels.Base
{
    public class EntityBase
    {
        public Guid Id { get; set; }
        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? CreatedBy { get; set; } = string.Empty;
        [Column(TypeName = "date")]
        public DateTime? UpdatedDate { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string? UpdatedBy { get; set; } = string.Empty;
        public bool DeleteFlag { get; set; }
        [Timestamp]
        public byte[]? Version { get; set; }
    }
}
