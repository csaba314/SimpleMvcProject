namespace Project.Common.ParamContainers
{
    public interface IFilteringParams
    {
        string CurrentFilter { get; set; }
        string SearchString { get; set; }
    }
}