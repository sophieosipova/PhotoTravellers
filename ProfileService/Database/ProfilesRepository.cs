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

            if (!db.Follows.Any())
            {
                db.Follows.Add(new Follow { FollowerId = "d968f867-cd4b-4f2c-915f-fd0bba4a06ba", FollowingId = "asdf43dw-cd4b-4f2c-915f-fd0bba4a06ba" });
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

        public async Task<List<Follow>> GetFollowers (string profileId)
        {
            try
            {
                return await db.Follows.Where(f => f.FollowingId == profileId).ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Follow>> GetFollowings(string profileId)
        {
            try
            {
                return await db.Follows.Where(f => f.FollowerId == profileId).ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Follow> CreateFollow(Follow follow)
        {
            var item = new Follow
            {
                FollowerId = follow.FollowerId,
                FollowingId = follow.FollowingId

            };

            try
            {
                db.Follows.Add(item);
                db.SaveChanges();

                return await db.Follows.LastAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Follow> DeleteFollow (int followId)
        {
            try
            {
                var follow = db.Follows.SingleOrDefault(f => f.FollowId == followId);

                if (follow == null)
                    return null;

                db.Follows.Remove(follow);
                await db.SaveChangesAsync();

                return follow;
            }
            catch
            {
                return null;
            }


        }


       
    }
}
