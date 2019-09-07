namespace Project.Common.ParamContainers
{
    public class FilteringParams : IFilteringParams
    {
        public string SearchString { get; set; }
        public string CurrentFilter { get; set; }
    }
}
