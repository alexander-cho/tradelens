namespace Core.Entities;

public class Stock : BaseEntity
{
    public required string Ticker { get; set; }
    public required string CompanyName { get; set; }
    public int? IpoYear { get; set; }
    public string? Country { get; set; }
    public string? Industry { get; set; }
    public string? Sector { get; set; }
}
