public class WeatherResponse
{
    public DateOnly Date { get; set; } 
    public int Temperature { get; set; } 
    public int FeelsLike { get; set; }
    public int HighTemperature { get; set; }
    public int LowTemperature { get; set; }   
    public string? Summary { get; set; } 
    public string? Location { get; set; }
    public int WindSpeed { get; set; }
    public int UvIndex { get; set; }
    public int RainChance { get; set; }
    public int Humidity  { get; set; }
}   