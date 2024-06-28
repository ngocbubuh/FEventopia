using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Enum;
using FEventopia.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public EventService(IEventRepository eventRepository, IMapper mapper, ILocationRepository locationRepository)
        {
            _eventRepository = eventRepository;
            _mapper = mapper;
            _locationRepository = locationRepository;
        }

        public async Task<EventOperatorModel> AddEventAsync(EventProcessModel eventProcessModel)
        {
            var eventModel = _mapper.Map<Event>(eventProcessModel); 
            eventModel.Status = EventStatus.INITIAL.ToString();
            var result = await _eventRepository.AddAsync(eventModel);
            return _mapper.Map<EventOperatorModel>(result);
        }

        public Task<bool> CancelEventAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteEventAsync(string id)
        {
            var result = await _eventRepository.GetByIdAsync(id);
            if (result == null)
            {
                return false;
            }
            return await _eventRepository.DeleteAsync(result);
        }

        public async Task<PageModel<EventModel>> GetAllEventAsync(PageParaModel pagePara)
        {
            var eventList = await _eventRepository.GetAllAsync();
            var result = _mapper.Map<List<EventModel>>(eventList);
            return PageModel<EventModel>.ToPagedList(result,
                pagePara.PageNumber,
                pagePara.PageSize);
        }

        public async Task<EventModel> GetEventByIdAsync(string id)
        {
            var result = await _eventRepository.GetEventWithDetailByIdAsync(id);
            return _mapper.Map<EventModel>(result);
        }

        public async Task<EventOperatorModel> GetEventByIdOperatorAsync(string id)
        {
            var result = await _eventRepository.GetEventWithDetailByIdAsync(id);
            return _mapper.Map<EventOperatorModel>(result);
        }

        public async Task<EventOperatorModel> UpdateEventAsync(string id, EventProcessModel eventProcessModel)
        {
            var eventCurrent = await _eventRepository.GetByIdAsync(id);
            if (eventCurrent == null) 
            {
                return null;
            }
            var eventUpdate = _mapper.Map(eventProcessModel, eventCurrent);
            var result = await _eventRepository.UpdateAsync(eventUpdate);
            if (!result)
            {
                return null;
            }
            return _mapper.Map<EventOperatorModel>(eventUpdate);
        }

        public async Task<bool> UpdateEventNextPhaseAsync(string id)
        {
            var eventModel = await _eventRepository.GetByIdAsync(id);
            if (eventModel == null)
            {
                return false;
            }
            switch (eventModel.Status.ToString())
            {
                case "INITIAL":
                    eventModel.Status = EventStatus.FUNDRAISING.ToString();
                    break;
                case "FUNDRAISING":
                    eventModel.Status = EventStatus.PREPARATION.ToString();
                    break;
                case "PREPARATION":
                    eventModel.Status = EventStatus.EXECUTE.ToString();
                    break;
                case "EXECUTE":
                    eventModel.Status = EventStatus.POST.ToString();
                    break;
                default:
                    return false;
            }
            await _eventRepository.UpdateAsync(eventModel);
            return true;
        }

        public async Task<EventModel> GetEventByName(string name)
        {
            var result = await _eventRepository.GetEventByName(name);
            return _mapper.Map<EventModel>(result);
        }

        public async Task<List<EventModel>> SearchEventByName(string name)
        {
            var results = await _eventRepository.SearchEventByName(name);
            return _mapper.Map<List<EventModel>>(results);
        }

    }
}
