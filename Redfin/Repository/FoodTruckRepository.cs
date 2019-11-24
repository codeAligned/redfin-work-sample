using Redfin.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redfin.Repository
{
    public class FoodTruckRepository
    {
        private readonly SocrataClient _client;
                
        public FoodTruckRepository()
        {
            _client = new SocrataClient();
        }
        
        /// <summary>
        ///     Gets the entire JSON file
        /// </summary>
        public async Task<IEnumerable<FoodTruck>> GetTrucks() => await _client.GetTrucks();
    }
}
