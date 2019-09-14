namespace Project.Common.ParamContainers
{
    public interface IParamsFactory
    {
        IFilteringParams FilteringParamsInstance(string searchString, string currentFilter);
        IOptions OptionsInstance(bool loadMakesWithModel);
        IPagingParams PagingParamsInstance(int pageNumber, int pageSize);
        ISortingParams SortingParamsInstance(string sorting);
    }
}