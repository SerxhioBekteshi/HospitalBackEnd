namespace Shared.RequestFeatures;

public class LookupRepositoryDTO
{
    public bool FromSearchAll { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public IEnumerable<LookupFilterDTO> Filters { get; set; } = new List<LookupFilterDTO>();
    public IEnumerable<LookupSortingDTO> Sorting { get; set; } = new List<LookupSortingDTO>();
}