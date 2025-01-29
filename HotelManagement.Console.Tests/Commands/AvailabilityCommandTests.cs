using System.Collections.Concurrent;
using HotelManagement.Console.Commands;
using HotelManagement.Console.Core;
using HotelManagement.Console.Model;

namespace HotelManagement.Console.Tests.Commands
{
    public class AvailabilityCommandTests
    {
        public AvailabilityCommandTests()
        {
            // Initialize GlobalData with some test data
            GlobalData.Hotels = new ConcurrentBag<Hotel>
            {
                new Hotel
                {
                    Id = "H1",
                    Name = "Hotel One",
                    Rooms = new List<Room>
                    {
                        new Room { RoomType = "DBL", RoomId = "101" },
                        new Room { RoomType = "DBL", RoomId="102" }
                    }
                }
            };

            GlobalData.Bookings = new ConcurrentBag<Booking>
            {
                new Booking
                {
                    HotelId = "H1",
                    RoomType = "DBL",
                    Arrival = DateTime.Parse("2024-10-05"),
                    Departure = DateTime.Parse("2024-10-06")
                }
            };            
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAvailableRooms_WhenRoomsAreAvailable()
        {
            // Arrange
            var command = new AvailabilityCommand("H1", "20241005-20241006", "DBL");

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            Assert.Equal("1", result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNoAvailableRooms_WhenNoRoomsAreAvailable()
        {
            // Arrange
            var command = new AvailabilityCommand("H1", "20241005-20241006", "SGL");

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            Assert.Equal("0", result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnHotelNotFound_WhenHotelDoesNotExist()
        {
            // Arrange
            var command = new AvailabilityCommand("H2", "20231005-20231006", "Single");

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync());
            Assert.Equal("Error during handling availability command: Invalid hotel ID", exception.Message);
        }

        [Fact]
        public void Constructor_ShouldThrowArgumentException_WhenDateRangeIsInvalid()
        {
            // Arrange, Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => new AvailabilityCommand("H1", "invalid-date-range", "Single"));
            Assert.Equal("Invalid date range format. Expected format: yyyyMMdd-yyyyMMdd", exception.Message);
        }
    }
}
