using System.Collections.Concurrent;
using HotelManagement.Console.Core;
using HotelManagement.Console.Model;
using HotelManagement.Console.Services.Implementations;
using HotelManagement.Console.Services.Interfaces;
using Moq;

namespace HotelManagement.Console.Tests.Services
{
    public class InitializationServiceTests
    {
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly Mock<IJsonService> _jsonServiceMock;
        private readonly InitializationService _initializationService;

        public InitializationServiceTests()
        {
            _fileServiceMock = new Mock<IFileService>();
            _jsonServiceMock = new Mock<IJsonService>();
            _initializationService = new InitializationService(_fileServiceMock.Object, _jsonServiceMock.Object);
        }

        [Fact]
        public async Task InitializeAsync_ShouldInitializeGlobalData_WhenCalled()
        {
            // Arrange
            var hotels = new ConcurrentBag<Hotel> { new Hotel { Id = "H1", Name = "Hotel One" } };
            var bookings = new ConcurrentBag<Booking> { new Booking { HotelId = "H1", RoomType = "Single", Arrival = DateTime.Today, Departure = DateTime.Today.AddDays(1) } };

            _jsonServiceMock.Setup(js => js.DeserializeJsonStreamAsync<ConcurrentBag<Hotel>>(It.IsAny<Stream>())).ReturnsAsync(hotels);
            _jsonServiceMock.Setup(js => js.DeserializeJsonStreamAsync<ConcurrentBag<Booking>>(It.IsAny<Stream>())).ReturnsAsync(bookings);

            // Act
            await _initializationService.InitializeAsync();

            // Assert
            Assert.NotNull(GlobalData.Hotels);
            Assert.Single(GlobalData.Hotels);
            Assert.Equal("H1", GlobalData.Hotels.First().Id);

            Assert.NotNull(GlobalData.Bookings);
            Assert.Single(GlobalData.Bookings);
            Assert.Equal("H1", GlobalData.Bookings.First().HotelId);
        }

        [Fact]
        public async Task InitializeAsync_ShouldThrowExceptionAndAllCollectionsShoulBeNull_WHenExceptionIsThrownInJsonService()
        {
            // Arrange
            _jsonServiceMock.Setup(js => js.DeserializeJsonStreamAsync<ConcurrentBag<Hotel>>(It.IsAny<Stream>())).ThrowsAsync(new InvalidOperationException("Test JsonService exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _initializationService.InitializeAsync());
            Assert.Equal("Test JsonService exception", exception.Message);
            Assert.Null(GlobalData.Bookings);
            Assert.Null(GlobalData.Hotels);
        }


        [Fact]
        public async Task InitializeAsync_ShouldThrowExceptionAndAllCollectionsShoulBeNull_WHenExceptionIsThrownInFileService()
        {
            // Arrange
            _fileServiceMock.Setup(js => js.OpenReadStreamAsync(It.IsAny<string>())).ThrowsAsync(new InvalidOperationException("Test FileService exception"));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _initializationService.InitializeAsync());
            Assert.Equal("Test FileService exception", exception.Message);
            Assert.Null(GlobalData.Bookings);
            Assert.Null(GlobalData.Hotels);
        }
    }
}
