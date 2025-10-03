namespace Core.Models;

public class MarketStatusModel
{
    public required string Exchange { get; set; }
    public string? Holiday { get; set; }
    public bool IsOpen { get; set; }
    public required string Session { get; set; }
    public required string Timezone { get; set; }
    public double T { get; set; }
}