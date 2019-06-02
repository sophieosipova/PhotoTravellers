using PhotoTravellers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public interface ILocationsService
    {
        Task<List<LocationModel>> GetCityLocations(string countryName, string cityName);
        Task<LocationModel> CreateLocation(LocationModel location);
        Task<LocationModel> DeleteLocation(int locationId);
        Task<LocationModel> UpdateLocation(LocationModel locationToUpdate);
    }
}
