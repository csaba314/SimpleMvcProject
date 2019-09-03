namespace Project.Service.Model
{
    using Project.Service.Model;
    using System;
    using System.Data.Entity;
    using System.Linq;

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