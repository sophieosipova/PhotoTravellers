using Microsoft.EntityFrameworkCore;
using ProfileService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProfileService.Database
{
    public class ProfilesRepository : IProfilesRepository
    {
        private readonly ProfilesContext db;

        public ProfilesRepository(ProfilesContext context)
        {
            this.db = context;
            if (!db.Profiles.Any())
            {
                db.Profiles.Add(new Profile { ProfileId = "d968f867-cd4b-4f2c-915f-fd0bba4a06ba", Login = "Sophie", FirstName = "Sophie", LastName = "Osipova", BirthDate = new DateTime(1996, 09, 26) });
                db.Profiles.Add(new Profile { ProfileId = "asdf43dw-cd4b-4f2c-915f-fd0bba4a06ba", Login = "Polina", FirstName = "Polina", LastName = "Osipova", BirthDate = new DateTime(1994, 06, 01) });

                db.SaveChanges();
            }
        }

        public async Task<Profile > GetProfileById(string profileId)
        {
            try
            {
                return await db.Profiles.SingleOrDefaultAsync(profile => profile.ProfileId == profileId);
            }
            catch
            {
                return null;
            }
        }

        public async Task<Profile> CreateProfile(Profile profile)
        {
            var item = new Profile
            {
                Login = profile.Login,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                BirthDate = profile.BirthDate,
                

            };

            try
            {
                db.Profiles.Add(item);
                db.SaveChanges();

                return await db.Profiles.LastAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Profile> DeleteProfile(string profileId)
        {
            try
            {
                var profile = db.Profiles.SingleOrDefault(p=> p.ProfileId == profileId);

                if (profile == null)
                    return null;

                db.Profiles.Remove(profile);
                await db.SaveChangesAsync();

                return profile;
            }
            catch
            {
                return null;
            }

           
        }


        public async Task<Profile> UpdateProfile(Profile profileToUpdate)
        {
            try
            {
                var profile = await db.Profiles
                    .SingleOrDefaultAsync(p => p.ProfileId == profileToUpdate.ProfileId);

                if (profile == null)
                    return null;

                profile.Login = profileToUpdate.Login;
                profile.FirstName = profileToUpdate.FirstName;
                profile.LastName = profileToUpdate.LastName;
                profile.BirthDate = profileToUpdate.BirthDate;

                db.Profiles.Update(profile);
                await db.SaveChangesAsync();

                return profile;
            }
            catch
            {
                return null;
            }

        }
    }
}
