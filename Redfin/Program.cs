using Redfin.Domain;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Redfin
{
    class Program
    {
        private static int offset = 0;
        private const int BATCH_SIZE = 10;

        /// <summary>
        ///     Write a command line program that prints out a list of food trucks that are open at the current date and current time, when the program is being run.
        ///     So if I run the program at noon on a Friday, I should see a list of all the food trucks that are open then.
        /// </summary>
        static async Task Main(string[] args)
        {
            //var time = DateTime.Parse("11/22/2019 1:30PM");
            var time = DateTime.Now;

            PrintTitle(time);
            await PrintFoodTrucks(time);
            PrintEndPrompt();
        }

        private static async Task PrintFoodTrucks(DateTime timeToCheck)
        {
            var foodTruckService = new Service.FoodTruckService();

            while (true)
            {
                var openFoodTrucks = await foodTruckService.GetBatchOpenFoodTrucks(timeToCheck, BATCH_SIZE, offset);

                if (!openFoodTrucks.Any())
                {
                    // No results to display, exit loop
                    break;
                }
                else
                {
                    // Print more results
                    PrintHeader();
                }

                foreach (var truck in openFoodTrucks)
                {
                    PrintFoodTruck(truck);
                }

                var input = Console.ReadLine(); // Allow user to break early
                if (input == "q") break; 
                else
                {
                    offset += BATCH_SIZE; // Increment offset by batch size
                }
            }
        }

        private static void PrintEndPrompt()
        {
            Console.WriteLine("\nHit any key to exit");
            Console.ReadKey();
        }

        private static void PrintTitle(DateTime time)
        {
            Console.WriteLine($"Food trucks open on {time.DayOfWeek} at {time.ToShortTimeString()}");
        }

        private static void PrintHeader()
        {
            Console.WriteLine("Name\tLocation\tDay\tHours");
        }

        private static void PrintFoodTruck(FoodTruck truck) => Console.WriteLine($"{truck.Name}\t{truck.Address}\t{truck.DayOfWeek}, {truck.OpeningTime.ToString()} - {truck.ClosingTime.ToString()}");
    }
}
