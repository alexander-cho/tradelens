namespace Core.DTOs.Polygon;

public class RelatedCompaniesDto
{
    public required string Ticker { get; set; }
    public string? Status { get; set; }

    public List<RelatedCompany>? Results { get; set; }
}

public class RelatedCompany
{
    public required string Ticker { get; set; }
}