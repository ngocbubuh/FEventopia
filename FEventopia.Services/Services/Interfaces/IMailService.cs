using FEventopia.Services.Settings;

namespace FEventopia.Services.Services.Interfaces
{
    public interface IMailService
    {
        public Task SendConfirmationEmailAsync(MailRequestSetting request);
        public Task SendTicketEmailAsync(MailRequestSetting request);
        public Task SendFeedbackEmailAsync(MailRequestSetting request);
        public Task SendAdvertisementEmailAsync(MailRequestSetting request);
    }
}
