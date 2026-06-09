using System.Text.Json.Serialization;

public class OpenWeatherResponse
{
    public string Name { get; set; }
    public MainData Main { get; set; }
    public WindData Wind { get; set; }
    public List<WeatherDescription> Weather { get; set; }
}

public class MainData
{
    public double Temp { get; set; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; set; }

    [JsonPropertyName("temp_min")]
    public double TempMin { get; set; }

    [JsonPropertyName("temp_max")]
    public double TempMax { get; set; }

    public int Humidity { get; set; }
}

public class WindData
{
    public double Speed { get; set; }
}

public class WeatherDescription
{
    public string Description { get; set; }
}