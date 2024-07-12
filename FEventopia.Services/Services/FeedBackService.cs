using AutoMapper;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.DAO.EntityModels;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FEventopia.Repositories.Repositories;
using FEventopia.Services.Enum;

namespace FEventopia.Services.Services
{
    public class FeedBackService : IFeedBackService
    {
        private readonly IFeedBackRepository _feedBackRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly IMapper _mapper;

        public FeedBackService(IFeedBackRepository feedBackRepository, IMapper mapper, IEventRepository eventRepository, IEventDetailRepository eventDetailRepository)
        {
            _feedBackRepository = feedBackRepository;
            _mapper = mapper;
            _eventRepository = eventRepository;
            _eventDetailRepository = eventDetailRepository;
        }

        public async Task<FeedBackModel> CreateFeedBack(FeedBackModel feedBackModel)
        {
            //lay eventdetail, kiem tra eventdetail có ton tai khum
            var eventdetail = await _eventDetailRepository.GetByIdAsync(feedBackModel.EventDetailID.ToString());
            if (eventdetail == null) { return null; } //chinh lai null

            //Lay event - Nếu sự kiện chưa ở giai đoạn post
            var @event = await _eventRepository.GetByIdAsync(eventdetail.EventID.ToString());
            if (!@event.Status.Equals(EventStatus.POST.ToString())) return null;

            var feedback = _mapper.Map<Feedback>(feedBackModel);
            var result = await _feedBackRepository.AddAsync(feedback);
            return _mapper.Map<FeedBackModel>(result); 
        }

        public async Task<bool> DeleteFeedBack(string Id)
        {
            var feedback = await _feedBackRepository.GetByIdAsync(Id);
            return await _feedBackRepository.DeleteAsync(feedback);
        }

        public async Task<PageModel<FeedBackModel>> GetAllByEventDetail(string eventDetailId, PageParaModel pageparamodel)
        {
            var feedbacks = await _feedBackRepository.GetAllByEventDetailId(eventDetailId);
            var result = _mapper.Map<List<FeedBackModel>>(feedbacks);
            return PageModel<FeedBackModel>.ToPagedList(result, pageparamodel.PageNumber, pageparamodel.PageSize);
        }

        public async Task<FeedBackModel> GetFeedBackByID(string id)
        {
            var feedback = await _feedBackRepository.GetByIdAsync(id);
            return _mapper.Map<FeedBackModel>(feedback);
        }

        public async Task<bool> UpdateFeedBack(string Id, FeedBackModel feedBackModel)
        {
            var feedback = await _feedBackRepository.GetByIdAsync(Id);

            //lay eventdetail, kiem tra eventdetail có ton tai khum
            var eventdetail = await _eventDetailRepository.GetByIdAsync(feedBackModel.EventDetailID.ToString());
            if (eventdetail == null) { return false; } //chinh lai null

            //Lay event - Nếu sự kiện chưa ở giai đoạn post
            var @event = await _eventRepository.GetByIdAsync(eventdetail.EventID.ToString());
            if (!@event.Status.Equals(EventStatus.POST.ToString())) return false;

            if (feedback == null)
            {
                return false;
            }
            var result = _mapper.Map(feedBackModel, feedback);
            return await _feedBackRepository.UpdateAsync(result);
        }
    }
}
