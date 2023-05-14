using System.Text.Json;

namespace Entities.ErrorModel;

public class ErrorDetails
{
    public bool Result { get; set; }
    public int StatusCode { get; set; }
    public string? Errors { get; set; }
    public override string ToString() => JsonSerializer.Serialize(this);
}