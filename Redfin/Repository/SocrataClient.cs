using Newtonsoft.Json;
using Redfin.Domain;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Redfin.Repository
{
    public class SocrataClient
    {
        private const string AppToken = "kJLid5Ruje1JT1LxpYiNzTifF";
        private readonly HttpClient _client;

        public SocrataClient()
        {
            _client = new HttpClient();
        }

        /// <summary>
        ///     https://dev.socrata.com/foundry/data.sfgov.org/jjew-r69b
        /// </summary>
        public async Task<IEnumerable<FoodTruck>> GetTrucks()
        {
            _client.DefaultRequestHeaders.TryAddWithoutValidation("X-App-Token", AppToken);

            var response = await _client.GetAsync("https://data.sfgov.org/resource/jjew-r69b.json");
            if (response.IsSuccessStatusCode)
            {
                // Received document from Socrata so deserialize it as enumerable of food trucks
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<FoodTruck>>(content);
            }

            // Return empty list
            return new List<FoodTruck>();
        }
    }
}
