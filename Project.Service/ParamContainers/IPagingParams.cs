namespace Project.Service.ParamContainers
{
    public interface IPagingParams
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
    }
}