using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProfileService.Models;

namespace ProfileService.Database
{
    public interface IProfilesRepository
    {
        Task<Profile> GetProfileById(string profileId);
        Task<Profile> CreateProfile(Profile profile);
        Task<Profile> DeleteProfile(string profileId);
        Task<Profile> UpdateProfile(Profile profileToUpdate);

        Task<List<Follow>> GetFollowers (string profileId);
        Task<List <Follow>> GetFollowings (string profileId);

        Task<Follow> CreateFollow (Follow follow);
        Task<Follow> DeleteFollow(int followId);
        //  Task<PaginatedModel<Comment>> GetCommentsByDishId(int dishId, int pageSize = 10, int pageIndex = 0);
    }
}
