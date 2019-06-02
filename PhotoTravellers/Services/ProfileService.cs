using Newtonsoft.Json;
using PhotoTravellers.Models;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public class ProfileService : IProfileService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "https://localhost:44383/api/profile";


        public ProfileService(/*HttpClient httpClient*/ string url)
        {

            this.httpClient = new HttpClient();

             this.remoteServiceBaseUrl = url+ "profile";
        }

       
        public async Task<ProfileModel> GetProfileById(string profileId)
        {
            var uri = $"{remoteServiceBaseUrl}/{profileId}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);


            if (response.IsSuccessStatusCode)
            {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    var profile = JsonConvert.DeserializeObject<ProfileModel>(responseBody);

                    return profile;
             }
            else
            {
                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                    throw new Exception($"{response.StatusCode }");
            }
            
 
            return null;
        }

        public async Task<ProfileModel> CreateProfile(ProfileModel profile)
        {
            var uri = $"{remoteServiceBaseUrl}";


            var profileContent = new StringContent(JsonConvert.SerializeObject(profile), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, profileContent);
            /*      if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                  {
                      await setHeaders();
                      response = await httpClient.PostAsync(uri, profileContent);
                  } */
            if (response.IsSuccessStatusCode)
            { 
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfileModel>(responseBody);
            }           
            else 
            {
                throw new Exception($"{response.StatusCode}");
            }


        }

        public async Task<ProfileModel> DeleteProfile(string profileId)
        {
            var uri = $"{remoteServiceBaseUrl}/{profileId}";


                var response = await httpClient.DeleteAsync(uri);

            /*  if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              {
                  await setHeaders();
                  response = await httpClient.DeleteAsync(uri);
              } */

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfileModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }

        }

        public async Task<ProfileModel> UpdateProfile (ProfileModel profileToUpdate)
        {
            var uri = $"{remoteServiceBaseUrl}/{profileToUpdate.ProfileId}";

                var profileContent = new StringContent(JsonConvert.SerializeObject(profileToUpdate), System.Text.Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync(uri, profileContent);
            /*   if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
               {
                   await setHeaders();
                   response = await httpClient.PutAsync(uri, productContent);
               } */
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ProfileModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }

        }


        public async Task<List<FollowModel>> GetFollowers(string profileId)
        {
            var uri = $"{remoteServiceBaseUrl}/{profileId}/followers";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var follows = JsonConvert.DeserializeObject<List<FollowModel>>(responseBody);

                return follows;
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }
        }
        public async Task<List<FollowModel>> GetFollowings(string profileId)
        {
            var uri = $"{remoteServiceBaseUrl}/{profileId}/followings";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var follows = JsonConvert.DeserializeObject<List<FollowModel>>(responseBody);

                return follows;
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }

        }

        public async Task<FollowModel> CreateFollow(string profileId, FollowModel follow)
        {
            var uri = $"{remoteServiceBaseUrl}/{profileId}/follow";


            var followContent = new StringContent(JsonConvert.SerializeObject(follow), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, followContent);
            /*      if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                  {
                      await setHeaders();
                      response = await httpClient.PostAsync(uri, profileContent);
                  } */
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FollowModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }
        }
        public async Task<FollowModel> DeleteFollow(int followId)
        {
            var uri = $"{remoteServiceBaseUrl}/{followId}";


            var response = await httpClient.DeleteAsync(uri);

            /*  if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              {
                  await setHeaders();
                  response = await httpClient.DeleteAsync(uri);
              } */

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FollowModel>(responseBody);
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
