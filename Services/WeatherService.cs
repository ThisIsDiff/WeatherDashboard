using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

public class WeatherService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _apiUrl;
    private readonly IDistributedCache _cache;

    public WeatherService(HttpClient httpClient, IConfiguration config, IDistributedCache cache)
    {
        _httpClient = httpClient;
        _apiKey = config["WeatherApi:ApiKey"];
        _apiUrl = config["WeatherApi:BaseUrl"];
        _cache = cache;
    }

    public async Task<WeatherResponse> GetWeatherAsync(string city)
    {
        WeatherResponse cityWeatherResponse = await GetCacheAsync(city);
        if (cityWeatherResponse is null) {
            var url = openURLbuilder(city);
            var openResponse = await _httpClient.GetFromJsonAsync<OpenWeatherResponse>(url);
            var cleanOpenResponse =  MapToWeatherResponse(openResponse);
            await SetCacheAsync(city, cleanOpenResponse);
            Console.WriteLine("Cache miss - calling openWeatherMap");
            return cleanOpenResponse;
        }
        Console.WriteLine("Cache hit - returning cache data");
        return cityWeatherResponse;
    }
    
    public async Task<WeatherResponse> GetCacheAsync(string city)
    {
        string weather = await _cache.GetStringAsync(city);
        if (weather is null) return null;
        return JsonSerializer.Deserialize<WeatherResponse>(weather);
    }

    public async Task SetCacheAsync(string city, WeatherResponse responseData)
    {
        var cacheExpirationTime = new DistributedCacheEntryOptions 
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        await _cache.SetStringAsync(city, JsonSerializer.Serialize(responseData), cacheExpirationTime);
    }

    public static WeatherResponse MapToWeatherResponse(OpenWeatherResponse openWeather)
    {
        return new WeatherResponse
        {
            Date = DateOnly.FromDateTime(DateTime.UtcNow),
            Temperature = (int)openWeather.Main.Temp,
            FeelsLike = (int)openWeather.Main.FeelsLike,
            HighTemperature = (int)openWeather.Main.TempMax,
            LowTemperature = (int)openWeather.Main.TempMin,
            Summary = openWeather.Weather[0].Description,
            Location = openWeather.Name,
            WindSpeed = (int)openWeather.Wind.Speed,
            UvIndex = 0, // Placeholder, as OpenWeather doesn't provide UV index in this endpoint
            RainChance = 0, // Placeholder, as OpenWeather doesn't provide rain chance in this
            Humidity = openWeather.Main.Humidity
        };
    }

    public string openURLbuilder(string city) 
    {
        return $"{_apiUrl}weather?q={city}&appid={_apiKey}&units=metric";
    }
}