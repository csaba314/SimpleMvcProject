namespace Project.Common.ParamContainers
{
    public interface IPagingParams
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}