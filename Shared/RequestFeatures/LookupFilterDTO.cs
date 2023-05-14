using Shared.Types;

namespace Shared.RequestFeatures;

public class LookupFilterDTO
{
    public string ColumnName { get; set; }
    public object Value { get; set; }
    public LookupFilterOperation Operation { get; set; }
}