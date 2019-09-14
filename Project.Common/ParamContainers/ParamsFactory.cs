namespace Project.Common.ParamContainers
{
    public class ParamsFactory : IParamsFactory
    {
        ISortingParams _sortingParams;
        IFilteringParams _filteringParams;
        IPagingParams _pagingParams;
        IOptions _options;

        public ParamsFactory(ISortingParams sortingParams,
                             IFilteringParams filteringParams,
                             IPagingParams pagingParams,
                             IOptions options)
        {
            _sortingParams = sortingParams;
            _filteringParams = filteringParams;
            _pagingParams = pagingParams;
            _options = options;
        }

        public ISortingParams SortingParamsInstance(string sorting)
        {
            _sortingParams.Sorting = sorting;
            return _sortingParams;
        }

        public IFilteringParams FilteringParamsInstance(string searchString, string currentFilter)
        {
            _filteringParams.SearchString = searchString;
            _filteringParams.CurrentFilter = currentFilter;
            return _filteringParams;
        }

        public IPagingParams PagingParamsInstance(int pageNumber, int pageSize)
        {
            _pagingParams.PageNumber = pageNumber;
            _pagingParams.PageSize = pageSize;
            return _pagingParams;
        }
        public IOptions OptionsInstance(string include)
        {
            _options.Include = include;
            return _options;
        }

    }
}
