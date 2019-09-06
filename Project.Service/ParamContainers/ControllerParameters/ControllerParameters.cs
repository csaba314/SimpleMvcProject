using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Containers
{
    public class ControllerParameters : IControllerParameters
    {
        public string SearchString { get; set; } 
        public string Sorting { get; set; }
        public string CurrentFilter { get; set; }
        //public int PageSize { get; set; }
        //public int PageNumber { get; set; }
        public IOptions Options { get; set; }
    }
}
