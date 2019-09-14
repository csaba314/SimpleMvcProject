using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.ParamContainers
{
    public class FilteringFactory : IFilteringFactory
    {
        private IFilteringParams _filteringParams;

        public FilteringFactory(IFilteringParams filteringParams)
        {
            _filteringParams = filteringParams;
        }

        public IFilteringParams Build(string searchString, string currentFilter)
        {
            _filteringParams.SearchString = searchString;
            _filteringParams.CurrentFilter = currentFilter;
            return _filteringParams;
        }
    }
}
