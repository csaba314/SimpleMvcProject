namespace Project.Service.Model
{
    using System.Data.Entity;

    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext()
            : base("ProjectDatabase")
        {
        }

        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<VehicleModel> VehicleModels { get; set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    
}