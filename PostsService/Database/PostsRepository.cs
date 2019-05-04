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
                db.Posts.Add(new Post { ProfileId = "d968f867-cd4b-4f2c-915f-fd0bba4a06ba", TimeStamp = new DateTime(2019, 05, 04, 23, 0, 0), UserName = "Sophie", Description = "First post", ImageURL = "Image1.jpg" });


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

        public async Task<bool> DeletePost(int postId)
        {
            var post = db.Posts.SingleOrDefault(x => x.PostId == postId);

            if (post == null)
                return false;

            try
            {
                db.Posts.Remove(post);
                await db.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
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


        public async Task<PaginatedModel<Post>> GetProfilesPosts (string profileId, int pageSize , int pageIndex )
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
    }
}

