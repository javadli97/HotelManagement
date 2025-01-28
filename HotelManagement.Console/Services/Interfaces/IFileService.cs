namespace HotelManagement.Console.Services.Interfaces
{
    public interface IFileService
    {
        Task<Stream> OpenReadStreamAsync(string filePath);
    }
}
