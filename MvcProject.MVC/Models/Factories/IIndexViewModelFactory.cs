namespace MvcProject.MVC.Models.Factories
{
    public interface IIndexViewModelFactory
    {
        IndexViewModel<VehicleMakeDTO, VehicleModelDTO> MakeIndexViewModelInstance();
        IndexViewModel<VehicleModelDTO, string> ModelIndexViewModelInstance();
    }
}