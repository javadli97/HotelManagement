using HotelManagement.Console.Services.Interfaces;
using Newtonsoft.Json;

namespace HotelManagement.Console.Services.Implementations
{
    public class JsonService : IJsonService
    {
        private readonly JsonSerializerSettings _settings;

        public JsonService(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public async Task<T> DeserializeJsonStreamAsync<T>(Stream jsonStream)
        {
            try
            {
                using (StreamReader sr = new StreamReader(jsonStream))
                using (JsonTextReader jsonReader = new JsonTextReader(sr))
                {
                    var serializer = JsonSerializer.Create(_settings);
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
