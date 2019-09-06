namespace Project.Service.ParamContainers
{
    public interface IParamsFactory
    {
        IFilteringParams FilteringParamsInstance();
        IOptions IOptionsInstance();
        IPagingParams PagingParamsInstance();
        ISortingParams SortingParamsInstance();
    }
}