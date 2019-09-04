namespace Project.Service.Model
{
    public interface IDomainModelFactory
    {
        IVehicleMake MakeInstance();
        IVehicleModel ModelInstance();
    }
}