using Microsoft.AspNetCore.Mvc;
[ApiController]
[Route("weather")]

public class WeatherController : ControllerBase
{
    private readonly WeatherService _weatherService;

    public WeatherController(WeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    [HttpGet("{city}")]
    public async Task<WeatherResponse> GetWeather(string city)
    {
        return await _weatherService.GetWeatherAsync(city);
    }

}