namespace Core.Specifications;

public class CompanyMetricSpecParams
{
    // private List<string> _tickers = [];
    //
    // public List<string> Tickers
    // {
    //     get => _tickers;
    //     
    //     set
    //     {
    //         _tickers = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
    //     }
    // }
    
    public required string Ticker { get; set; }

    public required string Interval { get; set; } = "quarterly";

    private List<string>? _metricList;
    public List<string>? MetricList
    {
        get => _metricList;
        set
        {
            _metricList = value?.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
}