namespace Project.Common.ParamContainers
{
    public interface IFilteringFactory
    {
        IFilteringParams Build(string searchString, string currentFilter);
    }
}