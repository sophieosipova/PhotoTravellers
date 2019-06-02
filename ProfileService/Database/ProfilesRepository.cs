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
                db.Profiles.Add(new Profile { ProfileId = "8a0a7e1b-9543-4c29-9db2-20ebad6527f7", Login = "Sophie", FirstName = "Sophie", LastName = "Osipova", BirthDate = new DateTime(1996, 09, 26), ImageUrl = "8a0a7e1b-9543-4c29-9db2-20ebad6527f7" });
                db.Profiles.Add(new Profile { ProfileId = "6e8e47f5-0d4d-4141-8716-bf1a34b5c546", Login = "Polina", FirstName = "Polina", LastName = "Osipova", BirthDate = new DateTime(1994, 06, 01), ImageUrl = "6e8e47f5-0d4d-4141-8716-bf1a34b5c546" });

                db.SaveChanges();
            }

            if (!db.Follows.Any())
            {
                db.Follows.Add(new Follow { FollowerId = "8a0a7e1b-9543-4c29-9db2-20ebad6527f7", FollowingId = "6e8e47f5-0d4d-4141-8716-bf1a34b5c546" });
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
                ProfileId = profile.ProfileId,
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
