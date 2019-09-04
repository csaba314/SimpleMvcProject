namespace MvcProject.MVC.Models.Factories
{
    public interface IDTOFactory
    {
        VehicleMakeDTO MakeDTOInstance();
        VehicleModelDTO ModelDTOInstance();
    }
}