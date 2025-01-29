using System.Collections.Concurrent;
using HotelManagement.Console.Commands;
using HotelManagement.Console.Core;
using HotelManagement.Console.Model;
using HotelManagement.Console.Requests;
using Moq;

namespace HotelManagement.Console.Tests.Commands
{
    public class AvailabilityCommandTests
    {
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;

        public AvailabilityCommandTests()
        {
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();

            // Initialize mock data
            var hotels = new ConcurrentBag<Hotel>
        {
            new Hotel
            {
                Id = "H1",
                Name = "Hotel One",
                Rooms = new List<Room>
                {
                    new Room { RoomType = "DBL", RoomId = "101" },
                    new Room { RoomType = "DBL", RoomId = "102" }
                }
            }
        };

            var bookings = new ConcurrentBag<Booking>
            {
                new Booking {HotelId = "H1", RoomType = "dbl", Arrival = new DateTime(2023,10,5), Departure = new DateTime(2023,10,6) }
            };

            _hotelRepositoryMock.Setup(repo => repo.GetHotels()).Returns(hotels);
            _bookingRepositoryMock.Setup(repo => repo.GetBookings()).Returns(bookings);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnAvailableRooms_WhenRoomsAreAvailable()
        {
            // Arrange
            var request = new AvailabilityRequest("H1", "20231005-20231006", "DBL");
            var command = new AvailabilityCommand(request, _hotelRepositoryMock.Object, _bookingRepositoryMock.Object);

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            Assert.Equal("1", result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnNoAvailableRooms_WhenNoRoomsAreAvailable()
        {
            // Arrange
            var request = new AvailabilityRequest("H1", "20231005-20231006", "SGL");
            var command = new AvailabilityCommand(request, _hotelRepositoryMock.Object, _bookingRepositoryMock.Object);

            // Act
            var result = await command.ExecuteAsync();

            // Assert
            Assert.Equal("0", result);
        }

        [Fact]
        public async Task ExecuteAsync_ShouldReturnHotelNotFound_WhenHotelDoesNotExist()
        {
            // Arrange
            var request = new AvailabilityRequest("H2", "20231005-20231006", "Single");
            var command = new AvailabilityCommand(request, _hotelRepositoryMock.Object, _bookingRepositoryMock.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync());
            Assert.Equal("Error during handling availability command: Invalid hotel ID", exception.Message);
        }
    }
}
