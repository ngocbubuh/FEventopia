using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Settings
{
    public class MailRequestSetting
    {
        [Required]
        public required string ToEmail { get; set; }
        [Required]
        public required string Subject { get; set; }
        [Required]
        public required string Body { get; set; }
    }
}
