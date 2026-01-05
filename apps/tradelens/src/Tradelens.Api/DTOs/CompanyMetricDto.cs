using Tradelens.Core.Entities;

namespace Tradelens.Api.DTOs;

public class CompanyMetricDto
{
    public required string MetricName { get; set; }
    public List<MetricDataPoint>? Data { get; set; }
}

public static class CompanyMetricMapper
{
    public static CompanyMetricDto ToCompanyMetricDto(List<CompanyMetric> companyMetrics)
    {
        if (companyMetrics[0].ParentMetric == companyMetrics[0].Metric)
        {
            return new CompanyMetricDto
            {
                MetricName = companyMetrics[0].Metric,
                Data = companyMetrics.Select(cm => new MetricDataPoint
                {
                    Period = cm.Period,
                    FiscalYear = cm.Year,
                    Value = cm.Value,
                    PeriodEndDate = cm.PeriodEndDate
                }).ToList()
            };
        }

        return new CompanyMetricDto
        {
            MetricName = companyMetrics[0].ParentMetric + "_" + companyMetrics[0].Metric,
            Data = companyMetrics.Select(cm => new MetricDataPoint
            {
                Period = cm.Period,
                FiscalYear = cm.Year,
                Value = cm.Value,
                PeriodEndDate = cm.PeriodEndDate
            }).ToList()
        };
    }
}