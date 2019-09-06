namespace Project.Service.Containers
{
    public interface IControllerParameters
    {
        string CurrentFilter { get; set; }
        IOptions Options { get; set; }
        string SearchString { get; set; }
        string Sorting { get; set; }
    }
}