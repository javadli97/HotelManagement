namespace HotelManagement.Console.Services.Interfaces
{
    public interface IJsonService
    {
        Task<T> DeserializeJsonStreamAsync<T>(Stream jsonStream);
    }
}
