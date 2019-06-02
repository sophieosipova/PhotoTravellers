using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostsService.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostsService.Database
{
    public class PostsRepository : IPostsRepository
    {
        private readonly PostsContext db;

        public PostsRepository(PostsContext context)
        {
            this.db = context;
            if (!db.Posts.Any())
             {
                db.Posts.Add(new Post { ProfileId = "8a0a7e1b-9543-4c29-9db2-20ebad6527f7", TimeStamp = new DateTime(2019, 05, 04, 23, 0, 0), UserName = "Sophie", Description = "First post", ImageURL = "Image1.jpg", LocationId = 1, LocationName = "Беларусь, Минск, Площадь Независимости" });
                db.Posts.Add(new Post { ProfileId = "6e8e47f5-0d4d-4141-8716-bf1a34b5c546", TimeStamp = new DateTime(2019, 05, 04, 23, 0, 0), UserName = "Polina", Description = "First post", ImageURL = "Image2.jpg", LocationId = 1, LocationName = "Беларусь, Минск, Площадь Независимости" });

                db.SaveChanges();
            }
        }                                                                                                                                 

    
        public async Task<List<Post>> GetProfilesPosts(string profileId)
        {
            try
            {
                return await db.Posts.Where(p => p.ProfileId == profileId).ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<PaginatedModel<Post>> GetProfilesPosts(string profileId, int pageSize, int pageIndex)
        {

            try
            {
                // int take = pageSize > 0 ? pageSize : db.Posts.Count();

                var totalItems = await db.Posts.Where(p => p.ProfileId == profileId)
                    .CountAsync();

                int take = pageSize > 0 ? pageSize : totalItems;

                var itemsOnPage = await db.Posts
                    .OrderBy(c => c.TimeStamp)
                    .Skip(pageSize * pageIndex)
                    .Take(take)
                    .ToListAsync();

                var model = new PaginatedModel<Post>(pageIndex, pageSize, totalItems, itemsOnPage);

                return model;
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Post>> GetPostsFromLocation (int locationId)
        {
            try
            {
                return await db.Posts.Where(p => p.LocationId== locationId).ToListAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<PaginatedModel<Post>> GetPostsFromLocation(int locationId, int pageSize, int pageIndex)
        {

            try
            {
                // int take = pageSize > 0 ? pageSize : db.Posts.Count();

                var totalItems = await db.Posts.Where(p => p.LocationId== locationId)
                    .CountAsync();

                int take = pageSize > 0 ? pageSize : totalItems;

                var itemsOnPage = await db.Posts
                    .OrderBy(c => c.TimeStamp)
                    .Skip(pageSize * pageIndex)
                    .Take(take)
                    .ToListAsync();

                var model = new PaginatedModel<Post>(pageIndex, pageSize, totalItems, itemsOnPage);

                return model;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Post> CreatePost([FromBody]Post post)
        {
            var item = new Post
            {
                ProfileId = post.ProfileId,
                Description = post.Description,
                ImageURL = post.ImageURL,
                TimeStamp = DateTime.Now,
                UserName = post.UserName
            };

            try
            {
                db.Posts.Add(item);
                db.SaveChanges();
                return await db.Posts.LastAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<Post> DeletePost(int postId)
        {
            var post = db.Posts.SingleOrDefault(x => x.PostId == postId);

            if (post == null)
                return null;

            try
            {
                db.Posts.Remove(post);
                await db.SaveChangesAsync();
                return post;
            }
            catch
            {
                return null;
            }

            
        }


        public async Task<Post> UpdatePost (Post postToUpdate)
        {
            var post = await db.Posts
                .SingleOrDefaultAsync(p => p.PostId == postToUpdate.PostId);

            if (post == null)
                return null;

            try
            {
                // product = productToUpdate;
                post.Description = postToUpdate.Description;
                post.ImageURL = postToUpdate.ImageURL;
                post.UserName = postToUpdate.UserName;
                db.Posts.Update(post);

                await db.SaveChangesAsync();

                return post;
            }
            catch
            {
                return null;
            }
        }


      
    }
}

