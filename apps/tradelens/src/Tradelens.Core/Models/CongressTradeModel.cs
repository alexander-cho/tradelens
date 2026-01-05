namespace Tradelens.Core.Models;

public class CongressTradeModel
{
    public required string DisclosureDate { get; set; }
    public required string TransactionDate { get; set; }
    public required string Office { get; set; }
    public string? Symbol { get; set; }
    public string? Owner { get; set; }
    public string? AssetDescription { get; set; }
    public string? AssetType { get; set; }
    public string? Type { get; set; }
    public string? Amount { get; set; }
    public string? Link { get; set; }
}