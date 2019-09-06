namespace Project.Service.Containers
{
    public interface IParamContainerBuilder
    {
        IControllerParameters BuildControllerParameters(string sorting, string searchString, int pageSize, int pageNumber, IOptions options = null);
        IOptions BuildLoadingOptions(bool loadMakesWithModel = false);
    }
}