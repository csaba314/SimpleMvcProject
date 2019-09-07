namespace Project.Common.ParamContainers
{
    public interface IParamsFactory
    {
        IFilteringParams FilteringParamsInstance();
        IOptions IOptionsInstance();
        IPagingParams PagingParamsInstance();
        ISortingParams SortingParamsInstance();
    }
}