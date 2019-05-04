using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Models
{
    public class ProfilesContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public ProfilesContext(DbContextOptions<ProfilesContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
