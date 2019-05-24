using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationService.Models
{

    public class LocationsContext : DbContext
    {
        public DbSet<Location> Locations { get; set; }
        public LocationsContext(DbContextOptions<LocationsContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
