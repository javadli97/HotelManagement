using System.Collections.Concurrent;
using HotelManagement.Console.Model;

namespace HotelManagement.Console.Core
{
    public class HotelRepository : IHotelRepository
    {
        public ConcurrentBag<Hotel> GetHotels() => GlobalData.Hotels;
    }
}
