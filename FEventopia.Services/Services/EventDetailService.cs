using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using FEventopia.Services.Utils;
using FEventopia.Services.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class EventDetailService : IEventDetailService
    {
        private readonly IEventDetailRepository _eventDetailRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMapper _mapper;

        public EventDetailService(IEventDetailRepository eventDetailRepository, IMapper mapper, ILocationRepository locationRepository, IEventRepository eventRepository)
        {
            _eventDetailRepository = eventDetailRepository;
            _mapper = mapper;
            _locationRepository = locationRepository;
            _eventRepository = eventRepository;
        }

        public async Task<EventDetailOperatorModel> AddEventDetailAsync(EventDetailProcessModel eventDetailModel)
        {
            if (eventDetailModel.EndDate < eventDetailModel.StartDate) { return null; }
            var existEventDetailAtLocation = await _eventDetailRepository.GetAllEventDetailAtLocation(eventDetailModel.LocationID.ToString(), 
                                                                                                        eventDetailModel.StartDate, 
                                                                                                        eventDetailModel.EndDate);
            //Tạo sự kiện => có thời gian bắt đầu phải cách thời gian hiện tại ít nhất 1 tuần
            if (TimeUtils.GetTimeVietNam().Date > eventDetailModel.StartDate.AddDays(7).Date) return null;

            //Kiểm tra có sự kiện ở vị trí đó cùng thời gian chưa
            foreach (var item in existEventDetailAtLocation)
            {
                if (eventDetailModel.StartDate < item.EndDate && eventDetailModel.EndDate > item.StartDate) return null;
            }
            var eventDetail = _mapper.Map<EventDetail>(eventDetailModel);

            //Get Event info
            var @event = await _eventRepository.GetByIdAsync(eventDetail.EventID.ToString());

            //Nếu ko tồn tại hoặc đang ko trong giai đoạn chuẩn bị
            if (@event == null || !@event.Status.Equals(EventStatus.PREPARATION.ToString()))
            {
                return null;
            }

            //Get Location info
            var location = await _locationRepository.GetByIdAsync(eventDetail.LocationID.ToString());
            if (location == null)
            {
                return null;
            }

            //Calculate Estimate Cost base on Event and Location
            EventCategory status = (EventCategory)System.Enum.Parse(typeof(EventCategory), @event.Category);
            switch (status)
            {
                case EventCategory.TALKSHOW:
                    eventDetail.EstimateCost = location.Capacity * 200000;
                    break;
                case EventCategory.MUSICSHOW:
                    eventDetail.EstimateCost = location.Capacity * 300000;
                    break;
                case EventCategory.FESTIVAL:
                    eventDetail.EstimateCost = location.Capacity * 400000;
                    break;
                case EventCategory.COMPETITION:
                    eventDetail.EstimateCost = location.Capacity * 500000;
                    break;
            }
            var result = await _eventDetailRepository.AddAsync(eventDetail);
            return _mapper.Map<EventDetailOperatorModel>(result);
        }

        public async Task<bool> DeleteEventDetailAsync(string id)
        {
            var result = await _eventDetailRepository.GetByIdAsync(id);
            if (result == null) { return false; }
            return await _eventDetailRepository.DeleteAsync(result);
        }

        public async Task<PageModel<EventDetailModel>> GetAllEventDetailAtLocation(string locationId, DateTime startDate, DateTime endDate, PageParaModel pagePara)
        {
            var eventDetailList = await _eventDetailRepository.GetAllEventDetailAtLocation(locationId, startDate, endDate);
            var result = _mapper.Map<List<EventDetailModel>>(eventDetailList);
            return PageModel<EventDetailModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<PageModel<EventDetailModel>> GetAllEventDetailByEventIdAsync(string eventId, PageParaModel pagePara)
        {
            var eventDetails = await _eventDetailRepository.GetAllEventDetailWithLocationById(eventId);
            var result = _mapper.Map<List<EventDetailModel>>(eventDetails);
            return PageModel<EventDetailModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<EventDetailModel> GetEventDetailByIdAsync(string id)
        {
            var result = await _eventDetailRepository.GetEventDetailWithLocationById(id);
            return _mapper.Map<EventDetailModel>(result);
        }

        public async Task<EventDetailOperatorModel> GetEventDetailOperatorByIdAsync(string id)
        {
            var result = await _eventDetailRepository.GetEventDetailWithLocationById(id);
            return _mapper.Map<EventDetailOperatorModel>(result);
        }

        public async Task<EventDetailOperatorModel> UpdateEventDetailAsync(string id, EventDetailProcessModel eventDetailModel)
        {
            if (eventDetailModel.EndDate < eventDetailModel.StartDate) { return null; }
            var existEventDetailAtLocation = await _eventDetailRepository.GetAllEventDetailAtLocation(eventDetailModel.LocationID.ToString(),
                                                                                                        eventDetailModel.StartDate,
                                                                                                        eventDetailModel.EndDate);
            //Tạo sự kiện => có thời gian bắt đầu phải cách thời gian hiện tại ít nhất 1 tuần
            if (TimeUtils.GetTimeVietNam().Date > eventDetailModel.StartDate.AddDays(7).Date) return null;

            //Kiểm tra có sự kiện ở vị trí đó cùng thời gian chưa
            foreach (var item in existEventDetailAtLocation)
            {
                if (eventDetailModel.StartDate < item.EndDate && eventDetailModel.EndDate > item.StartDate) return null;
            }

            var eventDetail = await _eventDetailRepository.GetByIdAsync(id);

            //Get Event info
            var @event = await _eventRepository.GetByIdAsync(eventDetail.EventID.ToString());

            //Nếu ko tồn tại hoặc đang ko trong giai đoạn chuẩn bị
            if (@event == null || !@event.Status.Equals(EventStatus.PREPARATION.ToString()))
            {
                return null;
            }
            
            if (eventDetail == null)
            {
                return null;
            }

            //Get Location info
            var location = await _locationRepository.GetByIdAsync(eventDetail.LocationID.ToString());
            if (location == null)
            {
                return null;
            }

            //Calculate Estimate Cost base on Event and Location
            EventCategory status = (EventCategory)System.Enum.Parse(typeof(EventCategory), @event.Category);
            switch (status)
            {
                case EventCategory.TALKSHOW:
                    eventDetail.EstimateCost = location.Capacity * 200000;
                    break;
                case EventCategory.MUSICSHOW:
                    eventDetail.EstimateCost = location.Capacity * 300000;
                    break;
                case EventCategory.FESTIVAL:
                    eventDetail.EstimateCost = location.Capacity * 400000;
                    break;
                case EventCategory.COMPETITION:
                    eventDetail.EstimateCost = location.Capacity * 500000;
                    break;
            }

            var eventDetailUpdate = _mapper.Map(eventDetailModel, eventDetail);
            var result = await _eventDetailRepository.UpdateAsync(eventDetailUpdate);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<EventDetailOperatorModel>(eventDetailUpdate);
        } 
    }
}
