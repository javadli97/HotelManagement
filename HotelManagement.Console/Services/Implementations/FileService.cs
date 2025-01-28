using HotelManagement.Console.Services.Interfaces;

namespace HotelManagement.Console.Services.Implementations
{
    public class FileService : IFileService
    {
        public async Task<Stream> OpenReadStreamAsync(string filePath)
        {
            try
            {
                return await Task.FromResult(File.OpenRead(filePath));
            }
            catch
            {
                throw new InvalidOperationException("Exception occured during reading from file");
            }
        }
    }
}
