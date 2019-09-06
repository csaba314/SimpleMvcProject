using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.ParamContainers
{
    public class FilteringParams : IFilteringParams
    {
        public string SearchString { get; set; }
        public string CurrentFilter { get; set; }
    }
}
