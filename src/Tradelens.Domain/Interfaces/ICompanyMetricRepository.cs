namespace Tradelens.Domain.Interfaces;

public interface ICompanyMetricRepository
{
    // Task<IEnumerable<CompanyMetric>> GetCompanyMetricsByTickerAsync(string ticker, string period);
    Task<IEnumerable<string>> GetListOfPossibleMetricsAsync(string ticker);
}