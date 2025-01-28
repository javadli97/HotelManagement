using Newtonsoft.Json;

namespace HotelManagement.Console.Core
{
    public static class GlobalSettings
    {
        private static readonly string baseDirectory = AppContext.BaseDirectory;

        public static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            DateFormatHandling = DateFormatHandling.IsoDateFormat,
            DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            DateFormatString = "yyyyMMdd"
        };

        public static string BookingPath { get; } = Path.Combine(baseDirectory, "data", "bookings.json");
        public static string HotelPath { get; } = Path.Combine(baseDirectory, "data", "hotels.json");

        
    }
}
