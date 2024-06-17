using FEventopia.Services.Settings;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IMailService
    {
        public Task SendEmailAsync(MailRequestSetting request);
    }
}
