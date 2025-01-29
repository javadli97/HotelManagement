using HotelManagement.Console.Services.Implementations;

namespace HotelManagement.Console.Tests.Services
{
    public class FileServiceTests
    {
        private readonly FileService _fileService;

        public FileServiceTests()
        {
            _fileService = new FileService();
        }

        [Fact]
        public async Task OpenReadStreamAsync_ShouldThrowFileNotFoundException_WhenFileDoesNotExist()
        {
            // Arrange
            string path = "nonexistentfile.txt";

            // Act
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => _fileService.OpenReadStreamAsync(path));

            // Assert
            Assert.Equal("Exception occured during reading from file", exception.Message);
        }

        [Fact]
        public async Task OpenReadStreamAsync_ShouldReturnFileStream_WhenFileExists()
        {
            // Arrange
            string path = "testfile.txt";
            await File.WriteAllTextAsync(path, "Test content");

            try
            {
                // Act
                using var stream = await _fileService.OpenReadStreamAsync(path);

                // Assert
                Assert.NotNull(stream);
                Assert.True(stream.CanRead);
            }
            finally
            {
                // Clean up
                File.Delete(path);
            }
        }
    }
}
