using Newtonsoft.Json;
using System;
using System.Globalization;

namespace Redfin.Domain
{
    [Serializable]
    public class FoodTruck
    {
        /// <summary>
        ///     Day of the week as an integer, starting with Sunday as 0 through Saturday as 6
        /// </summary>
        [JsonProperty(PropertyName = "dayorder")]
        public int DayOrder { get; set; }

        /// <summary>
        ///     String representation of day of the week
        /// </summary>
        [JsonProperty(PropertyName = "dayofweekstr")]
        public string DayOfWeek { get; set; }

        /// <summary>
        ///     Military time representation of opening time 0:00 -> 24:00
        /// </summary>
        [JsonProperty(PropertyName = "start24")]
        public string StartTime { get; set; }

        /// <summary>
        ///     Note: this attribute does not come from the JSON response, but would like a TimeSpan object from time comparisons
        ///     Parse opening time from start24
        ///     Assumption: StartTime is always populated
        /// </summary>
        public TimeSpan OpeningTime => ParseTimeOfDay(StartTime);

        /// <summary>
        ///     Military time representation of closing time 0:00 -> 24:00
        /// </summary>
        [JsonProperty(PropertyName = "end24")]
        public string EndTime { get; set; }

        /// <summary>
        ///     Note: this attribute does not come from the JSON response, but would like a TimeSpan object from time comparisons
        ///     Parse closing time from end24
        ///     Assumption: EndTime is always populated
        /// </summary>
        public TimeSpan ClosingTime => ParseTimeOfDay(EndTime);

        /// <summary>
        ///     Renaming location from JSON property to Address here
        /// </summary>
        [JsonProperty(PropertyName = "location")]
        public string Address { get; set; }

        /// <summary>
        ///     Renaming application from JSON property to Name here
        /// </summary>
        [JsonProperty(PropertyName = "applicant")]
        public string Name { get; set; }

        /// <summary>
        ///     Parse military time to a DateTime object (today)
        /// </summary>
        private TimeSpan ParseTimeOfDay(string militaryTime)
        {
            if (militaryTime == "24:00")
            {
                // TimeSpan.Parse doesn't handle 24:00
                militaryTime = "00:00";
            }

            // Doesn't matter what day
            var dateTimeString = $"{DateTime.Now.ToShortDateString()} {militaryTime}";

            return TimeSpan.Parse(militaryTime);
        }
    }


    public static class FoodTruckExtensions
    {
        /// <summary>
        ///     Is the food truck open on this date and time?
        ///     If no time argument provided, assume current
        /// </summary>
        public static bool IsOpen(this FoodTruck foodTruck, DateTime? time = null)
        {
            var timeToCheck = time ?? DateTime.Now;

            // Open on this day?
            if (foodTruck.DayOrder != (int)timeToCheck.DayOfWeek) return false;

            // Open at this time?
            return timeToCheck.TimeOfDay > foodTruck.OpeningTime && timeToCheck.TimeOfDay < foodTruck.ClosingTime;
        }
    }
}


