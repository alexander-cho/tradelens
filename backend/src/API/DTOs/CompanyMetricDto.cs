using Core.Entities;

namespace API.DTOs;

/* example: user asks for Revenue, NetIncome, OriginationVolume, EPS
 * First two are single bar, OriginationVolume has sub-metrics PersonalLoans, StudentLoans, HomeLoans, EPS has sub-metrics Basic, Diluted
 * {
 *      MetricData: [
 *          {
 *              MetricName: Revenue,
 *              Data: [{Period: "Q1", FiscalYear: "2021", Value: 100000}, {}, {},...],
 *              ChildMetrics: null
 *          },
 *          {
 *              MetricName: NetIncome,
 *              Data: [{Period: "Q1", FiscalYear: "2021", Value: 50000}, {}, {},...],
 *              ChildMetrics: null
 *          },
 *          {
 *              MetricName: OriginationVolume,
 *              Data: null,
 *              ChildMetrics: [
 *                  {
 *                      MetricName: "PersonalLoans",
 *                      Data: [{Period: "Q1", FiscalYear: "2021", Value: 345000}, {}, {},...]
 *                  },
 *                  {
 *                      MetricName: "StudentLoans",
 *                      Data: [{Period: "Q1", FiscalYear: "2021", Value: 155000}, {}, {},...]
 *                  },
 *                  {
 *                      MetricName: "HomeLoans",
 *                      Data: [{Period: "Q1", FiscalYear: "2021", Value: 75000}, {}, {},...]
 *                  }
 *              ]
 *          },
 *          {
 *              MetricName: EPS,
 *              Data: null,
 *              ChildMetrics: [
 *                  {
 *                      MetricName: "Diluted",
 *                      Data: [{Period: "Q1", FiscalYear: "2021", Value: 0.02}, {}, {},...]
 *                  },
 *                  {
 *                      MetricName: "Basic",
 *                      Data: [{Period: "Q1", FiscalYear: "2021", Value: 0.01}, {}, {},...]
 *                  }
 *              ]
 *          }
 *      ]
 * }
 */

public class CompanyMetricDto
{
    public List<MetricDataGroup>? MetricData { get; set; }
}

public class MetricDataGroup
{
    public string? MetricName { get; set; }  // parent metric name
    public List<MetricDataPoint>? Data { get; set; }  // for flat metrics (Revenue, NetIncome)
    public List<ChildMetricGroup>? ChildMetrics { get; set; }  // for hierarchical metrics (OriginationVolume)
}

public class ChildMetricGroup
{
    public string? MetricName { get; set; }  // PersonalLoans, HomeLoans, StudentLoans.
    public List<MetricDataPoint>? Data { get; set; }
}

public class MetricDataPoint
{
    public string? Period { get; set; }
    public int FiscalYear { get; set; }
    public decimal Value { get; set; }
    public DateOnly PeriodEndDate { get; set; }
}

public static class CompanyMetricMapper
{
    public static CompanyMetricDto ToCompanyMetricDto(List<CompanyMetric> companyMetrics)
    {
        var metricDataGroups = new List<MetricDataGroup>();
    
        // group by ParentMetric
        var groupedByParent = companyMetrics.GroupBy(x => x.ParentMetric);
        
        // for each one determine if it is flat (ParentMetric == Metric) or hierarchical (ParentMetric != Metric)
        foreach (var parentMetricGroup in groupedByParent)
        {
            var parentMetricName = parentMetricGroup.Key;
        
            // check if this parent group is flat or hierarchical
            // it's flat if ALL items have Metric == ParentMetric
            var isFlat = parentMetricGroup.All(x => x.Metric == x.ParentMetric);

            if (isFlat)
            {
                var metricDataGroup = new MetricDataGroup
                {
                    MetricName = parentMetricName,
                    ChildMetrics = null,
                    Data = parentMetricGroup.Select(cm => new MetricDataPoint
                    {
                        Period = cm.Period,
                        FiscalYear = cm.Year,
                        Value = cm.Value,
                        PeriodEndDate = cm.PeriodEndDate
                    }).ToList()
                };
                
                metricDataGroups.Add(metricDataGroup);
            }
            else
            {
                // group by Metric (child metric name) within this parent
                var childGroups = parentMetricGroup.GroupBy(x => x.Metric);
                var childMetricsList = new List<ChildMetricGroup>();
                foreach (var childGroup in childGroups)
                {
                    var childMetricGroup = new ChildMetricGroup
                    {
                        MetricName = childGroup.Key,
                        Data = childGroup.Select(cm => new MetricDataPoint
                        {
                            Period = cm.Period,
                            FiscalYear = cm.Year,
                            Value = cm.Value,
                            PeriodEndDate = cm.PeriodEndDate
                        }).ToList()
                    };
                    childMetricsList.Add(childMetricGroup);
                }
                
                var metricDataGroup = new MetricDataGroup
                {
                    MetricName = parentMetricName,
                    Data = null,
                    ChildMetrics = childMetricsList
                };
                metricDataGroups.Add(metricDataGroup);
            }
        }


        return new CompanyMetricDto
        {
            MetricData = metricDataGroups
        };
    }
}