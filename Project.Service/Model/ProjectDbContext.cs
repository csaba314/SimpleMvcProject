namespace Project.Service.Model
{
    using System.Data.Entity;

    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext()
            : base("ProjectDatabase")
        {
        }

        public ProjectDbContext GetDbContext()
        {
            return new ProjectDbContext();
        }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }
    }
}