using PhotoTravellers.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public interface IPostsService : IDisposable
    {
        Task<List<PostModel>> GetProfilesPosts(string profileId);
        Task<PaginatedModel<PostModel>> GetProfilesPosts(string profileId, int pageSize, int pageIndex);

        Task<List<PostModel>> GetPostsFromLocation(int locationId);
        Task<PaginatedModel<PostModel>> GetPostsFromLocation(int locationId, int pageSize, int pageIndex);

        //Task<List<Post>> GetLocationPosts(string locationId);
        Task<PostModel> CreatePost(string profileId, PostModel post);
        Task<PostModel> DeletePost(string profileId, int postId);
        Task<PostModel> UpdatePost(string profileId, PostModel postToUpdate);
        
    }
}
