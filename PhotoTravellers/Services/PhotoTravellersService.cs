using PhotoTravellers.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public class PhotoTravellersService : IPhotoTravellersService
    {
        private readonly IPostsService postsService;
        private readonly IProfileService profileService;
      
        public PhotoTravellersService(IPostsService postsService, IProfileService profileService)
        {
            this.postsService = postsService;
            this.profileService = profileService;
        }
        public async Task<List<PostModel>> GetFeed(string profileId)
        {
            try
            {
                var followings = await  profileService.GetFollowings(profileId);
                List<PostModel> posts = new List<PostModel>();
                foreach (FollowModel followin in followings)
                {
                    posts.AddRange(await postsService.GetProfilesPosts(followin.FollowingId));
                }

                return posts.OrderBy(s => s.TimeStamp).ToList();
                
            }
            catch(Exception e)
            {
                throw e;
            }

        }

        public async Task<PaginatedModel<PostModel>> GetFeed(string profileId, int pageSize, int pageIndex)
        {
            try
            {
                var followings = await profileService.GetFollowings(profileId);
                List<PostModel> posts = new List<PostModel>();
                foreach (FollowModel followin in followings)
                {
                    posts.AddRange(await postsService.GetProfilesPosts(followin.FollowingId));
                }

                var totalItems = posts.Count();
                int take = pageSize > 0 ? pageSize : totalItems;

                var itemsOnPage = posts
                    .OrderBy(c => c.TimeStamp)
                    .Skip(pageSize * pageIndex)
                    .Take(take);

                var model = new PaginatedModel<PostModel>(pageIndex, pageSize, totalItems, itemsOnPage);
                return model;

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
