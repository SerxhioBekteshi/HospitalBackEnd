namespace Shared.ResponseFeatures;

public class BaseResponse<TData, TErrors>
{
    public bool Result { get; set; }
    public TData Data { get; set; }
    public TErrors Errors { get; set; }
    public int StatusCode { get; set; }
}