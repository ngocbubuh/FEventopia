using AutoMapper;
using FEventopia.DAO.EntityModels;
using FEventopia.Repositories.Repositories.Interfaces;
using FEventopia.Services.BussinessModels;
using FEventopia.Services.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FEventopia.Services.Services
{
    public class LocationService : ILocationService
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;

        public LocationService(ILocationRepository locationRepository, IMapper mapper)
        {
            _locationRepository = locationRepository;
            _mapper = mapper;
        }


        public async Task<LocationModel> CreateLocation(LocationModel model)
        {
            var location = _mapper.Map<Location>(model);
            var result = await _locationRepository.AddAsync(location);
            return _mapper.Map<LocationModel>(result);
        }

        public async Task<bool> DeleteLocation(string name)
        {
            var location = await _locationRepository.GetByNameAsync(name);
            return await _locationRepository.DeleteAsync(location);
        }

        public async Task<bool> UpdateLocation(LocationModel model)
        {
            var location = await _locationRepository.GetByNameAsync(model.LocationName);
            var result = _mapper.Map(model, location);
            return await _locationRepository.UpdateAsync(result);
        }

        public async Task<List<LocationModel>> GetAllLocation()
        {
            var listLocation = await _locationRepository.GetAllAsync();
            return _mapper.Map<List<LocationModel>>(listLocation);
        }

        public async Task<LocationModel> GetLocationByName(string name)
        {
            var location = await _locationRepository.GetByNameAsync(name);
            return _mapper.Map<LocationModel>(location);
        }
    }
}
