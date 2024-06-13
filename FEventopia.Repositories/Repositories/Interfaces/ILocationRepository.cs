using FEventopia.DAO.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Repositories.Repositories.Interfaces
{
    public interface ILocationRepository : IGenericRepository<Location>
    {
        public Task<Location> GetByNameAsync(string name);
    }
}
