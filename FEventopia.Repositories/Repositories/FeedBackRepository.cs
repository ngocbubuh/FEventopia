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
    public class FeedBackRepository : GenericRepository<Feedback>, IFeedBackRepository
    {
        private readonly IGenericDAO<Feedback> _feedbackDAO;
        public FeedBackRepository(IGenericDAO<Feedback> genericDAO) : base(genericDAO)
        {
            _feedbackDAO = genericDAO;
        }

        public async Task<List<Feedback>> GetAllByEventDetail(string eventDetailId)
        {
            var feedBacks = await _feedbackDAO.GetAllAsync();
            string idtoupper = eventDetailId.ToLower();
            //return feedBacks.Where(f => f.EventDetailId.Equals(eventDetailId)).ToList();
            return feedBacks.Where(f => idtoupper.Equals(f.EventDetailId.ToString())).ToList();
        }
        
    }
}
