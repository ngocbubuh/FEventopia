using FEventopia.Services.BussinessModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services.Interfaces
{
    public interface ILocationService
    {
        public Task<List<LocationModel>> GetAllLocation();
        public Task<LocationModel> GetLocationByName(string name);
        public Task<LocationModel> CreateLocation(LocationModel location);
        public Task<bool> UpdateLocation(LocationModel location);
        public Task<bool> DeleteLocation(string name);
    }
}
