using FEventopia.DAO.DAO.Interfaces;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories
{
    public class EventAssigneeRepository : GenericRepository<EventAssignee>, IEventAssigneeRepository
    {
        private readonly IGenericDAO<EventAssignee>  _EventAssigneeDAO;

        public EventAssigneeRepository(IGenericDAO<EventAssignee> eventAssigneeDAO) : base(eventAssigneeDAO)
        {
            _EventAssigneeDAO = eventAssigneeDAO;
        }

        public async Task<List<EventAssignee>> GetAllByEventDetail(string eventDetailIdSAMPLE)
        {
            var eventassignees = await _EventAssigneeDAO.GetAllAsync();
            string eventDetailId = eventDetailIdSAMPLE.ToLower();
            return eventassignees.Where(e => e.EventDetailId.ToString().Equals(eventDetailId)).ToList();
        }
        public async Task<EventAssignee> GetByED_AC(string eventdetailIdSAMPLE, string accountId)
        {
            var eventassignee = await _EventAssigneeDAO.GetAllAsync();
            string eventdetailId = eventdetailIdSAMPLE.ToLower();
            //string accountId = accountIdSAMPLE.ToLower();
            return eventassignee.Where(e => e.EventDetailId.ToString().Equals(eventdetailId) && e.AccountId.Equals(accountId)).SingleOrDefault();
        }

        public async Task<List<EventAssignee>> GetEventAssigneeByAccountId(string accountid)
        {
            var evenAssignee = await _EventAssigneeDAO.GetAllAsync();
            return evenAssignee.Where(ea => ea.AccountId.ToString().ToLower().Equals(accountid)).ToList();
        }
    }
}
