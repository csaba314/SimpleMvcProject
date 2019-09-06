namespace Project.Service.ParamContainers
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

        public ISortingParams SortingParamsInstance()
        {
            return _sortingParams;
        }

        public IFilteringParams FilteringParamsInstance()
        {
            return _filteringParams;
        }

        public IPagingParams PagingParamsInstance()
        {
            return _pagingParams;
        }
        public IOptions IOptionsInstance()
        {
            return _options;
        }

    }
}
