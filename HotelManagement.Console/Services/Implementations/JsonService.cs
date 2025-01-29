using HotelManagement.Console.Core;
using HotelManagement.Console.Services.Interfaces;
using Newtonsoft.Json;

namespace HotelManagement.Console.Services.Implementations
{
    public class JsonService : IJsonService
    {
        public async Task<T> DeserializeJsonStreamAsync<T>(Stream jsonStream)
        {
            try
            {
                using (StreamReader sr = new StreamReader(jsonStream))
                using (JsonTextReader jsonReader = new JsonTextReader(sr))
                {
                    var serializer = JsonSerializer.Create(GlobalSettings.JsonSettings);
                    return await Task.Run(() => serializer.Deserialize<T>(jsonReader));
                }
            }
            catch
            {
                throw new InvalidOperationException("Exception occured during JSON deserialization");
            }
        }
    }
}
