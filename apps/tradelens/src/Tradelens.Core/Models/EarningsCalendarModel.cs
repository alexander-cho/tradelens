namespace Tradelens.Core.Models;

// actual values optional in case earnings haven't happened yet

public class EarningsCalendarModel
{
    public required string Symbol { get; set; }
    public required string Date { get; set; }
    public float? EpsActual { get; set; }
    public float EpsEstimated { get; set; }
    public float? RevenueActual { get; set; }
    public float RevenueEstimated { get; set; }
}