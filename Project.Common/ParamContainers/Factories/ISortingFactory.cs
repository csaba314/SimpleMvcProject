namespace Project.Common.ParamContainers
{
    public interface ISortingFactory
    {
        ISortingParams Build(string sorting);
    }
}