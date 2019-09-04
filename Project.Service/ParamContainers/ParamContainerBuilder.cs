using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Containers
{
    public class ParamContainerBuilder : IParamContainerBuilder
    {
        private IControllerParameters _parameters;
        private ILoadingOptions _options;

        public ParamContainerBuilder(
            IControllerParameters controllerParameters, 
            ILoadingOptions loadingOptions)
        {
            _parameters = controllerParameters;
            _options = loadingOptions;
        }

        public IControllerParameters BuildControllerParameters(
            string sorting, string searchString, int pageSize, int pageNumber, ILoadingOptions options)
        {
            _parameters.Sorting = sorting;
            _parameters.SearchString = searchString;
            //_parameters.PageSize = pageSize,
            //_parameters.PageNumber = pageNumber
            _parameters.Options = options ?? BuildLoadingOptions();
            
            return _parameters;
        }

        public ILoadingOptions BuildLoadingOptions(bool loadMakesWithModel = false)
        {
            _options.LoadMakesWithModel = loadMakesWithModel;
            return _options;
        }
    }
}
