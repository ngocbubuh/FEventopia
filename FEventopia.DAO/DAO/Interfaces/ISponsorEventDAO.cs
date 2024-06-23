using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.DAO.DAO.Interfaces
{
    public interface ISponsorEventDAO : IGenericDAO<SponsorEvent>
    {
        public Task<SponsorEvent?> GetSponsorEventDetail(string id);
        public Task<List<SponsorEvent>> GetAllSponsorEventDetail();
    }
}
