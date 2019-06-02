using Newtonsoft.Json;
using PhotoTravellers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PhotoTravellers.Services
{
    public class LocationsService : ILocationsService
    {
        private readonly HttpClient httpClient;
        private readonly string remoteServiceBaseUrl = "https://localhost:44394/api/locations";


        public LocationsService(/*HttpClient httpClient*/ string url)
        {

            this.httpClient = new HttpClient();

            this.remoteServiceBaseUrl = url+ "locations";
        }

        public async Task<List<LocationModel>> GetCityLocations (string countryName, string cityName)
        {
            var uri = $"{remoteServiceBaseUrl}/country/{countryName}/city/{cityName}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                var locations = JsonConvert.DeserializeObject<List<LocationModel>>(responseBody);

                return locations;
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }
        }
        public async Task<LocationModel> CreateLocation(LocationModel location)
        {
            var uri = $"{remoteServiceBaseUrl}/location/{location.LocationId}";


            var locationContent = new StringContent(JsonConvert.SerializeObject(location), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(uri, locationContent);
            /*      if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                  {
                      await setHeaders();
                      response = await httpClient.PostAsync(uri, profileContent);
                  } */
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LocationModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }


        }

        public async Task<LocationModel> DeleteLocation(int locationId)
        {
            var uri = $"{remoteServiceBaseUrl}/location/{locationId}";


            var response = await httpClient.DeleteAsync(uri);

            /*  if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              {
                  await setHeaders();
                  response = await httpClient.DeleteAsync(uri);
              } */

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LocationModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }

        }

        public async Task<LocationModel> UpdateLocation(LocationModel locationToUpdate)
        {
            var uri = $"{remoteServiceBaseUrl}/location/{locationToUpdate.LocationId}";

            var postContent = new StringContent(JsonConvert.SerializeObject(locationToUpdate), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PutAsync(uri, postContent);
            /*   if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
               {
                   await setHeaders();
                   response = await httpClient.PutAsync(uri, productContent);
               } */
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<LocationModel>(responseBody);
            }
            else
            {
                throw new Exception($"{response.StatusCode}");
            }

        }
    }
}
