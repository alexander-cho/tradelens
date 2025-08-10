namespace Core.DTOs.Fmp;

public class RevenueSegmentation
{
    public required string Symbol { get; set; }
    public required int FiscalYear { get; set; }
    public required string Period { get; set; }
    public required Dictionary<string, int> Data { get; set; }
}