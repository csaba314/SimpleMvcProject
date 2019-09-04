namespace Project.Service.Containers
{
    public interface IParamContainerBuilder
    {
        IControllerParameters BuildControllerParameters(string sorting, string searchString, int pageSize, int pageNumber, ILoadingOptions options = null);
        ILoadingOptions BuildLoadingOptions(bool loadMakesWithModel = false);
    }
}