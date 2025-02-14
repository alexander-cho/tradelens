namespace Core.Specifications;

public class StockSpecParams
{
    private List<int?> _ipoYears = [];

    public List<int?> IpoYears
    {
        get => _ipoYears;
        set
        {
            // convert each to a string, then split, then back to original type of int
            _ipoYears = value
                .Select(x => x.ToString())
                .SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .Select(x => (int?)int.Parse(x.Trim()))
                .ToList();
        }
    }


    private List<string?> _countries = [];

    public List<string?> Countries
    {
        get => _countries;
        set
        {
            _countries = value.SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }


    private List<string?> _sectors = [];

    public List<string?> Sectors
    {
        get => _sectors;
        set
        {
            _sectors = value.
                Where(x => x != null).
                SelectMany(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries)).ToList();
        }
    }
    
    
    public string? Sort { get; set; }
    
    
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