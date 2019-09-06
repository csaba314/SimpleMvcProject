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
        private IOptions _options;

        public ParamContainerBuilder(
            IControllerParameters controllerParameters, 
            IOptions loadingOptions)
        {
            _parameters = controllerParameters;
            _options = loadingOptions;
        }

        public IControllerParameters BuildControllerParameters(
            string sorting, string searchString, int pageSize, int pageNumber, IOptions options)
        {
            _parameters.Sorting = sorting;
            _parameters.SearchString = searchString;
            //_parameters.PageSize = pageSize,
            //_parameters.PageNumber = pageNumber
            _parameters.Options = options ?? BuildLoadingOptions();
            
            return _parameters;
        }

        public IOptions BuildLoadingOptions(bool loadMakesWithModel = false)
        {
            _options.LoadMakesWithModel = loadMakesWithModel;
            return _options;
        }
    }
}
