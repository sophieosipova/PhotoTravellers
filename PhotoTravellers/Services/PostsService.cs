using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PhotoTravellers.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public class PostsService : IPostsService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "https://localhost:44319/api/posts";


        public PostsService(/*HttpClient httpClient*/ string url)
        {

            this.httpClient = new HttpClient();

            this.remoteServiceBaseUrl = url;
        }

        public async Task<List<PostModel>> GetProfilesPosts(string profileId)
        {
            var uri = $"{remoteServiceBaseUrl}posts/profile/{profileId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<PostModel>>(responseBody);

                return posts;
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }
        }


        public async Task<PaginatedModel<PostModel>> GetProfilesPosts (string profileId, int pageSize, int pageIndex)
        {
            var uri = $"{remoteServiceBaseUrl}posts/profile/{profileId}?pageSize={pageSize}&pageIndex={pageIndex}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<PaginatedModel<PostModel>>(responseBody);

                return posts;
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }
        }

        public async Task<List<PostModel>> GetPostsFromLocation(int locationId)
        {
            var uri = $"{remoteServiceBaseUrl}posts/location/{locationId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<List<PostModel>>(responseBody);

                return posts;
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }
        }


        public async Task<PaginatedModel<PostModel>> GetPostsFromLocation(int locationId, int pageSize, int pageIndex)
        {
            var uri = $"{remoteServiceBaseUrl}posts/location/{locationId}?pageSize={pageSize}&pageIndex={pageIndex}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var posts = JsonConvert.DeserializeObject<PaginatedModel<PostModel>>(responseBody);

                return posts;
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }
        }

        public async Task<PostModel> CreatePost (string profileId, PostModel post)
        {
            var uri = $"{remoteServiceBaseUrl}posts/profile/{profileId}";


            var postContent = new StringContent(JsonConvert.SerializeObject(post), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, postContent);
            /*      if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                  {
                      await setHeaders();
                      response = await httpClient.PostAsync(uri, profileContent);
                  } */
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PostModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }


        }

        public async Task<PostModel> DeletePost(string profileId, int postId)
        {
            var uri = $"{remoteServiceBaseUrl}posts/profile/{profileId}/post/{postId}";


            var response = await httpClient.DeleteAsync(uri);

            /*  if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              {
                  await setHeaders();
                  response = await httpClient.DeleteAsync(uri);
              } */

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PostModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }

        }

        public async Task<PostModel> UpdatePost (string profileId, PostModel profileToUpdate)
        {
            var uri = $"{remoteServiceBaseUrl}posts/profile/{profileId}";

            var postContent = new StringContent(JsonConvert.SerializeObject(profileToUpdate), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(uri, postContent);
            /*   if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
               {
                   await setHeaders();
                   response = await httpClient.PutAsync(uri, productContent);
               } */
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PostModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }

        }

        public async Task<byte[]> GetPicture(string url)
        {
            var uri = $"{remoteServiceBaseUrl}picture/{url}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                byte [] responseBody = await response.Content.ReadAsByteArrayAsync();
                return responseBody;
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }
        }
        public void Dispose()

        {
            httpClient.Dispose();
        }

    }
}
