using Shared.Types;

namespace Shared.RequestFeatures;

public class LookupSortingDTO
{
    public string ColumnName { get; set; }
    public LookupSortingDirection Direction { get; set; }
}