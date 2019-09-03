﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Containers
{
    public static class ContainerBuilder
    {
        public static IControllerParameters BuildControllerParameters(
            string sorting, string searchString, int pageSize, int pageNumber, ILoadingOptions options = null)
        {
            var parameters = new ControllerParameters
            {
                Sorting = sorting,
                SearchString = searchString,
                //PageSize = pageSize,
                //PageNumber = pageNumber
                Options = options ?? BuildLoadingOptions()
            };
            return parameters;
        }

        public static ILoadingOptions BuildLoadingOptions(bool loadMakesWithModel = false)
        {
            return new Options { LoadMakesWithModel = loadMakesWithModel };
        }
    }
}
