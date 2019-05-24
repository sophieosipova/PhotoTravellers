using System.Collections.Generic;
using System.Threading.Tasks;
using LocationService.Models;


namespace LocationService.Database
{
    public interface ILocationsRepository
    {
        Task<List<Location>> GetCityLocations(string cityName);
        Task<Location> CreateLocation(Location location);
        Task<Location> DeleteLocation(int locationId);
        Task<Location> UpdateLocation(Location locationToUpdate);
    }
}



