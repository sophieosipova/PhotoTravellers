using PostsService.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsService.Database
{
    public interface IPostsRepository
    {
        Task<List<Post>> GetProfilesPosts (string profileId);
        //Task<List<Post>> GetLocationPosts(string locationId);
        Task<Post> CreatePost(Post post);
        Task<bool> DeletePost(int postId);
        Task<Post> UpdatePost( Post postToUpdate);
        Task<PaginatedModel<Post>> GetProfilesPosts(string profileId, int pageSize, int pageIndex);
    }
}
