using Newtonsoft.Json;
using SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public class AutorizationService : IAutorizationService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "https://localhost:44304/api/authorization";

        public AutorizationService(/*HttpClient httpClient*/ string url)
        {
            //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            HttpClientHandler handler = new HttpClientHandler();
            handler.AllowAutoRedirect = false;
            this.httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            remoteServiceBaseUrl = url + "authorization";
        }
        public async Task<UsersToken> Login(User user)
        {
            var uri = $"{remoteServiceBaseUrl}";
            var userContent = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, userContent);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var usersToken = JsonConvert.DeserializeObject<UsersToken>(responseBody);

                return usersToken;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }
        public async Task<UsersToken> RefreshTokens(UsersToken token)
        {
            var uri = $"{remoteServiceBaseUrl}/refreshtokens";

            //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            //   HttpResponseMessage response = await httpClient.GetAsync(uri,);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token.RefreshToken);
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

            try
            {
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                var usersToken = JsonConvert.DeserializeObject<UsersToken>(responseBody);

                return usersToken;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;

        }

        public async Task<bool> ValidateUserToken(string accessToken)
        {
            var uri = $"{remoteServiceBaseUrl}/user";

            if (accessToken == "")
                return false;
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{accessToken.Remove(0, 7)}");
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);


                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }

            //   return false;

        }

        public async Task<bool> ValidateAdminToken(string accessToken)
        {
            var uri = $"{remoteServiceBaseUrl}/admin";

            if (accessToken == "")
                return false;
            try
            {
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", $"{accessToken.Remove(0, 7)}");
                HttpResponseMessage response = await httpClient.SendAsync(requestMessage);


                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException e)
            {
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }

            //   return false;

        }

      

        public async Task<bool> Logout (string accessToken)
        {
            var uri = $"{remoteServiceBaseUrl}/logout";

            //   httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token.RefreshToken}");
            //   HttpResponseMessage response = await httpClient.GetAsync(uri,);
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await httpClient.SendAsync(requestMessage);

            try
            {
                response.EnsureSuccessStatusCode();
             

                return true;
            }
            catch (HttpRequestException e)
            {
                if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                    throw e;
            }
            catch (Exception e)
            {
                throw e;
            }

            return false;
        } 


    }

}
