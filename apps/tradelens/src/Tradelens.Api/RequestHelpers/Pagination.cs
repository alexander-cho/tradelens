namespace Tradelens.Api.RequestHelpers;


// specify pagination properties to return to client to sort UI
public class Pagination<T>(int pageIndex, int pageSize, int count, IReadOnlyList<T> data)
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;

    // total number of items fitting criteria to return before pagination
    public int Count { get; set; } = count;
    public IReadOnlyList<T>? Data { get; set; } = data;
}
