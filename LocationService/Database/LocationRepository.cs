using LocationService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationService.Database
{
    public class LocationsRepository : ILocationsRepository
    {
        private readonly LocationsContext db;

        public LocationsRepository(LocationsContext context)
        {
            this.db = context;
            if (!db.Locations.Any())
            {
                db.Locations.Add(new Location { LocationCity = "Минск", LocationName = "Площадь Независимости",
                    FromLongitude = 53.895556f, FromLatitude = 27.547778f,
                    ToLongitude = 53.895556f,
                    ToLatitude = 27.547778f
                });


                db.SaveChanges();
            }
        }


        public async Task<List<Location>> GetCityLocations(string locationCity)
        {
            try
            {
                return await db.Locations.Where(l => l.LocationCity == locationCity).ToListAsync();
            }
            catch
            {
                return null;
            }
        }


        public async Task<Location> CreateLocation([FromBody]Location location)
        {
            var item = new Location
            {
                LocationCity = location.LocationCity,
                LocationName = location.LocationName,
                FromLatitude = location.FromLatitude,
                FromLongitude = location.FromLongitude,
                ToLatitude = location.ToLatitude,
                ToLongitude = location.FromLongitude
                
            };

            try
            {
                db.Locations.Add(item);
                await db.SaveChangesAsync();
                return item;
            }
            catch
            {
                return null;
            }



        }

        public async Task<Location> DeleteLocation(int locationId)
        {
            var location = db.Locations.SingleOrDefault(x => x.LocationId == locationId);

            if (location == null)
                return null;

            try
            {
                db.Locations.Remove(location);
                await db.SaveChangesAsync();
                return location;
            }
            catch
            {
                return null;
            }


        }


        public async Task<Location> UpdateLocation(Location locationToUpdate)
        {
            var location = await db.Locations
                .SingleOrDefaultAsync(l => l.LocationId == locationToUpdate.LocationId);

            if (location == null)
                return null;

            try
            {
                location.LocationCity = locationToUpdate.LocationCity;
                location.LocationName = locationToUpdate.LocationName;
                location.ToLatitude = locationToUpdate.ToLatitude;
                location.ToLongitude = locationToUpdate.ToLongitude;
                location.FromLatitude = locationToUpdate.FromLatitude;
                location.FromLongitude = locationToUpdate.FromLongitude;

                await db.SaveChangesAsync();

                return location;
            }
            catch
            {
                return null;
            }
        }
    }

}
