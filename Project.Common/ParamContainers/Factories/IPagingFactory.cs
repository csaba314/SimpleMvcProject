namespace Project.Common.ParamContainers
{
    public interface IPagingFactory
    {
        IPagingParams Build(int pageNumber, int pageSize);
    }
}