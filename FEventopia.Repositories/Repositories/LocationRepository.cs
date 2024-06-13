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
    public class LocationRepository : GenericRepository<Location>, ILocationRepository
    {
        private readonly IGenericDAO<Location> _locationDAO;

        public LocationRepository(IGenericDAO<Location> genericDAO) : base(genericDAO)
        {
            _locationDAO = genericDAO;
        }

        public async Task<Location> GetByNameAsync(string name)
        {
            var location = await _locationDAO.GetAllAsync();
            return location.Where(p => name.Equals(p.LocationName)).SingleOrDefault();
        }
    }
}
