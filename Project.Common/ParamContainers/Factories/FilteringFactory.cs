using System;

namespace Project.Common.ParamContainers
{
    public class FilteringFactory : IFilteringFactory
    {
        private IFilteringParams _filteringParams;

        public FilteringFactory(IFilteringParams filteringParams)
        {
            _filteringParams = filteringParams ?? throw new ArgumentNullException(nameof(IFilteringParams));
        }

        public IFilteringParams Build(string searchString, string currentFilter)
        {
            _filteringParams.SearchString = searchString;
            _filteringParams.CurrentFilter = currentFilter;
            return _filteringParams;
        }
    }
}
