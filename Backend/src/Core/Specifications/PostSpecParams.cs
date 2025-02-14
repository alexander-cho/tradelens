namespace Core.Specifications;


// creating a specification params object as a way to pass in to controller method instead of individual strings
// let user filter by more than one type, i.e. more than one ticker
public class PostSpecParams
{
    private List<string> _tickers = [];
    public List<string> Tickers
    {
        get => _tickers;
        // query string for ex: .../api/...?tickers=SOFI,TSLA,AFRM&...
        set
        {
            _tickers = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    
    private List<string> _sentiments = [];
    public List<string> Sentiments
    {
        get => _sentiments;
        set
        {
            _sentiments = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }

    public string? Sort { get; set; }

    // pagination
    // set maximum number of items per page, initialize page index and size
    private const int MaxPageSize = 50;
    public int PageIndex { get; set; } = 1;
    private int _pageSize = 5;
    public int PageSize
    {
        get => _pageSize;
        // if it exceeds Max set to max otherwise specified value
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }

    // search
    private string? _search;
    public string Search
    {
        get => _search ?? "";
        set => _search = value?.ToLower();
    }
}
