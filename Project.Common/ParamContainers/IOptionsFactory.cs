namespace Project.Common.ParamContainers
{
    public interface IOptionsFactory
    {
        IOptions Build(string include);
    }
}