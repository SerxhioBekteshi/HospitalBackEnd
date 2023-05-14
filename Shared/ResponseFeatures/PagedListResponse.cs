namespace Shared.ResponseFeatures;

public class PagedListResponse<T>
{
    public List<DataTableColumn> Columns { get; set; }
    public T Rows { get; set; }
    public List<TotalColumn> Totals { get; set; }
    public int TotalCount { get; set; }
    public int CurrentPage { get; set; }
    //public int RowCount { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public string Key { get; set; }
    public bool HasNext { get; set; }
    public bool HasPrevious { get; set; }

}