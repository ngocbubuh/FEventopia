using System.ComponentModel.DataAnnotations;

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
