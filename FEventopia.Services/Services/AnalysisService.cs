using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class AnalysisService : IAnalysisService
    {
        private readonly ITicketRepository _ticketRepository; //TicketSold, TicketIncome, TicketCheckedIn
        private readonly IEventStallRepository _eventStallRepository; //StallSold, StallIncome
        private readonly ITaskRepository _taskRepository; //Expense
        private readonly IFeedBackRepository _feedBackRepository; //Feedback
        private readonly IEventRepository _eventRepository; //InitialCapital, SponsorCapital

        public AnalysisService(ITicketRepository ticketRepository, IEventStallRepository eventStallRepository, ITaskRepository taskRepository, IFeedBackRepository feedBackRepository, IEventRepository eventRepository)
        {
            _ticketRepository = ticketRepository;
            _eventStallRepository = eventStallRepository;
            _taskRepository = taskRepository;
            _feedBackRepository = feedBackRepository;
            _eventRepository = eventRepository;
        }

        public async Task<AnalysisModel> GetEventAnalysis(string eventId)
        {
            var @event = await _eventRepository.GetEventWithDetailByIdAsync(eventId);
            if (@event == null) return null;

            var ticketList = new List<Ticket>();
            var stallList = new List<EventStall>();
            var taskList = new List<DAO.EntityModels.Task>();
            var feedbackList = new List<Feedback>();

            foreach (var eventD in @event.EventDetail)
            {
                ticketList.AddRange(await _ticketRepository.GetAllTicketWithDetailCurrentEvent(eventD.Id.ToString()));
                stallList.AddRange(await _eventStallRepository.GetAllEventStallByEventDetailId(eventD.Id.ToString()));
                taskList.AddRange(await _taskRepository.GetAllByEventDetailId(eventD.Id.ToString()));
                feedbackList.AddRange(await _feedBackRepository.GetAllByEventDetailId(eventD.Id.ToString()));
            }

            var analysisModel = new AnalysisModel
            {
                InitialCapital = @event.InitialCapital,
                SponsorCaptital = @event.Status.Equals(EventStatus.CANCELED.ToString()) ? 0 : @event.SponsorCapital,
                NumTicketSold = ticketList.Count,
                NumTicketCheckedIn = ticketList.Where(t => t.CheckInStatus).ToList().Count,
                TicketIncome = @event.Status.Equals(EventStatus.CANCELED.ToString()) ? 0 : @event.TicketSaleIncome,
                NumStallSold = stallList.Count,
                StallIncome = @event.Status.Equals(EventStatus.CANCELED.ToString()) ? 0 : @event.StallSaleIncome,
                AverageFeedback = !feedbackList.IsNullOrEmpty() ? feedbackList.Sum(feedback => feedback.Rate) / feedbackList.Count : 0,
                ActualExpense = taskList.Sum(task => task.ActualCost)
            };

            return analysisModel;
        }
    }
}
