namespace Core.Specifications;

public class TradierOptionChainSpecParams
{
    public required string Symbol { get; set; }
    public required string Expiration { get; set; }
    public bool? Greeks { get; set; } = false;
}