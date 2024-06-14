using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
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
            var eventDetail = _mapper.Map<EventDetail>(eventDetailModel);

            //Get Event info
            var @event = await _eventRepository.GetByIdAsync(eventDetail.EventID.ToString());
            if (@event == null)
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
            switch (@event.Category)
            {
                case "TALKSHOW":
                    eventDetail.EstimateCost = location.Capacity * 50000;
                    break;
                case "MUSICSHOW":
                    eventDetail.EstimateCost = location.Capacity * 100000;
                    break;
                case "FESTIVAL":
                    eventDetail.EstimateCost = location.Capacity * 150000;
                    break;
                case "COMPETITION":
                    eventDetail.EstimateCost = location.Capacity * 200000;
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
            var eventDetail = await _eventDetailRepository.GetByIdAsync(id);
            if (eventDetail == null)
            {
                return null;
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
