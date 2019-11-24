using Redfin.Domain;
using Redfin.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace Redfin.Service
{
    public class FoodTruckService
    {
        private readonly FoodTruckRepository _foodTruckRepository;
        
        /// <summary>
        ///     Result set is valid for 30 min
        /// </summary>
        private const int CacheDurationMin = 30;

        public FoodTruckService()
        {
            _foodTruckRepository = new FoodTruckRepository();
        }

        /// <summary>
        ///     Get batch of food trucks that are currently open
        /// </summary>
        public async Task<IEnumerable<FoodTruck>> GetBatchOpenFoodTrucks(DateTime time, int limit, int offset = 0)
        {
            var openFoodTrucks = await GetOpenFoodTrucks(time);            
            return openFoodTrucks.Skip(offset).Take(limit);
        }

        /// <summary>
        ///     Retrieves result set of currently open food trucks (either stored from cache, or pulls fresh from repository)
        /// </summary>
        private async Task<IEnumerable<FoodTruck>> GetOpenFoodTrucks(DateTime time)
        {
            var memoryCache = MemoryCache.Default;
            var cacheKey = "GET_BATCH_OPEN_FOOD_TRUCKS";

            if (memoryCache.Contains(cacheKey))
            {
                // Get from cache if it exists
                return memoryCache.Get(cacheKey) as IEnumerable<FoodTruck>;
            }
            else
            {
                var currentlyOpenFoodTrucks = await GetOpenFoodTrucksFromRepository(time);

                // Cache the result set for 30 minutes
                var expiration = DateTimeOffset.Now.AddMinutes(CacheDurationMin);
                memoryCache.Add(cacheKey, currentlyOpenFoodTrucks, expiration);

                return currentlyOpenFoodTrucks;
            }
        }

        /// <summary>
        ///     Retrieves entire set of food trucks (including those that are not open)
        ///     Filters by those that are open, then sorts by name ascending
        /// </summary>
        private async Task<IEnumerable<FoodTruck>> GetOpenFoodTrucksFromRepository(DateTime timeToCheck)
        {
            var allFoodTrucks = await _foodTruckRepository.GetTrucks();

            // Filter by open
            var openFoodTrucks = allFoodTrucks.Where(truck => truck.IsOpen(timeToCheck));

            // Sort by name
            return openFoodTrucks.OrderBy(x => x.Name);
        }
    }
}
