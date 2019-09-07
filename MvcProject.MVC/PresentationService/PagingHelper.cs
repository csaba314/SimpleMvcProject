using System.Collections.Generic;

namespace MvcProject.MVC.PresentationService
{
    public static class PagingHelper
    {
        public static IEnumerable<int> PageSizeDropdown {
            get
            {
                return new List<int>() { 5, 10, 20, 40 };
            }
        }
    }
}