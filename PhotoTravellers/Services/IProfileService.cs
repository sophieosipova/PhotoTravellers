using PhotoTravellers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public interface IProfileService : IDisposable
    {
        Task<ProfileModel> GetProfileById(string profileId);
        Task<ProfileModel> CreateProfile(ProfileModel profile);
        Task<ProfileModel> DeleteProfile(string profileId);
        Task<ProfileModel> UpdateProfile(ProfileModel profileToUpdate);

        Task<List<FollowModel>> GetFollowers(string profileId);
        Task<List<FollowModel>> GetFollowings(string profileId);

        Task<FollowModel> CreateFollow(string profileId, FollowModel follow);
        Task<FollowModel> DeleteFollow(int followId);
    }
}
