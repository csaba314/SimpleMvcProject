namespace Project.Common.ParamContainers
{
    public interface IParamsFactory
    {
        IFilteringParams FilteringParamsInstance(string searchString, string currentFilter);
        IPagingParams PagingParamsInstance(int pageNumber, int pageSize);
        ISortingParams SortingParamsInstance(string sorting);
        IOptions OptionsInstance(string include);
    }
}