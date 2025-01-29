using System.Collections.Concurrent;
using HotelManagement.Console.Model;

namespace HotelManagement.Console.Core
{
    public static class GlobalData
    {
        public static ConcurrentBag<Hotel> Hotels { get; set; }
        public static ConcurrentBag<Booking> Bookings { get; set; }
    }
}
