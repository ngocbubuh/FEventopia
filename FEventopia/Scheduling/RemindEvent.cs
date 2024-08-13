using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Settings;
using FEventopia.Services.Utils;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Quartz;

namespace FEventopia.Controllers.Scheduling
{
    public class RemindEvent : IJob
    {
        private readonly IEventDetailService _eventDetailService;
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;
        private readonly IMailService _mailService;
        public RemindEvent(IEventDetailService eventDetailService, IUserService userService, IMailService mailService, ITicketService ticketService)
        {
            _eventDetailService = eventDetailService;
            _userService = userService;
            _mailService = mailService;
            _ticketService = ticketService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var tomorrow = TimeUtils.GetTimeVietNam().AddDays(1);
            var eventDetails = await _eventDetailService.GetAllEventDetailByStartDate(tomorrow);
            foreach (var eventDetail in eventDetails)
            {
                //Get all ticket of that event
                var tickets = await _ticketService.GetAllTicketWithDetailCurrentEvent(eventDetail.Id);

                //If the user had been remind about the event, skip that user
                var sentAccountId = new HashSet<string>();
                foreach (var ticket in tickets)
                {
                    if (!sentAccountId.Contains(ticket.VisitorID))
                    {
                        //Get the owner of the ticket
                        var user = await _userService.GetByIdAsync(ticket.VisitorID);
                        var messageRequest = new MailRequestSetting
                        {
                            ToEmail = user.Email,
                            Subject = "FEventopia Remind Email",
                            Body = RemindEventEmail.EmailContent(ticket, user)
                        };

                        //Send mail
                        await _mailService.SendEmailAsync(messageRequest);

                        //This user has been inform about this event
                        sentAccountId.Add(ticket.VisitorID);
                    }
                }
            }
        }
    }
}
