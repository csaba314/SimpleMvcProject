namespace Project.Service.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using Project.Service.Model;
    using System.Linq;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<ProjectDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ProjectDbContext context)
        {
            var makeList = new List<VehicleMake>
            {
                new VehicleMake { Name="Bavarian Motor Works", Abrv="BMW" },
                new VehicleMake { Name="Mercedes-Benz", Abrv="Mercedes" },
                new VehicleMake { Name="Alfa Romeo", Abrv="ALFA" },
                new VehicleMake { Name="Audi AG", Abrv="AUDI" },
                new VehicleMake { Name="Volkswagen", Abrv="VW" },
                new VehicleMake { Name="General Motors Company", Abrv="GMC" },
                new VehicleMake { Name="Kia Motors Corporation", Abrv="KIA" }
            };
            makeList.ForEach(make => context.VehicleMakes.AddOrUpdate(x => x.Name, make));
            context.SaveChanges();

            var modelList = new List<VehicleModel>
            {
                new VehicleModel{ Name="M3", Abrv=makeList.Single(x => x.Name == "Bavarian Motor Works").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Bavarian Motor Works").Id},
                new VehicleModel{ Name="M5", Abrv=makeList.Single(x => x.Name == "Bavarian Motor Works").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Bavarian Motor Works").Id},
                new VehicleModel{ Name="X3 M", Abrv=makeList.Single(x => x.Name == "Bavarian Motor Works").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Bavarian Motor Works").Id},
                new VehicleModel{ Name="X5", Abrv=makeList.Single(x => x.Name == "Bavarian Motor Works").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Bavarian Motor Works").Id},
                new VehicleModel{ Name="C-Class", Abrv = makeList.Single(x => x.Name == "Mercedes-Benz").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Mercedes-Benz").Id},
                new VehicleModel{ Name="E-Class", Abrv = makeList.Single(x => x.Name == "Mercedes-Benz").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Mercedes-Benz").Id},
                new VehicleModel{ Name="GLC-Class", Abrv = makeList.Single(x => x.Name == "Mercedes-Benz").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Mercedes-Benz").Id},
                new VehicleModel{ Name="GLS", Abrv = makeList.Single(x => x.Name == "Mercedes-Benz").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Mercedes-Benz").Id},
                new VehicleModel{ Name="GTV", Abrv=makeList.Single(x => x.Name == "Alfa Romeo").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Alfa Romeo").Id},
                new VehicleModel{ Name="8C", Abrv=makeList.Single(x => x.Name == "Alfa Romeo").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Alfa Romeo").Id},
                new VehicleModel{ Name="A3", Abrv = makeList.Single(x => x.Name == "Audi AG").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Audi AG").Id},
                new VehicleModel{ Name="A5", Abrv = makeList.Single(x => x.Name == "Audi AG").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Audi AG").Id},
                new VehicleModel{ Name="E-Tron", Abrv = makeList.Single(x => x.Name == "Audi AG").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Audi AG").Id},
                new VehicleModel{ Name="Golf", Abrv = makeList.Single(x => x.Name == "Volkswagen").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Volkswagen").Id},
                new VehicleModel{ Name="Jetta", Abrv = makeList.Single(x => x.Name == "Volkswagen").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Volkswagen").Id},
                new VehicleModel{ Name="Passat", Abrv = makeList.Single(x => x.Name == "Volkswagen").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Volkswagen").Id},
                new VehicleModel{ Name="T-Roc", Abrv = makeList.Single(x => x.Name == "Volkswagen").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Volkswagen").Id},
                new VehicleModel{ Name="Touareg", Abrv = makeList.Single(x => x.Name == "Volkswagen").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Volkswagen").Id},
                new VehicleModel{ Name="Canyon", Abrv = makeList.Single(x => x.Name == "General Motors Company").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "General Motors Company").Id},
                new VehicleModel{ Name="Sierra", Abrv = makeList.Single(x => x.Name == "General Motors Company").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "General Motors Company").Id},
                new VehicleModel{ Name="Rio", Abrv = makeList.Single(x => x.Name == "Kia Motors Corporation").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Kia Motors Corporation").Id},
                new VehicleModel{ Name="Sportage", Abrv = makeList.Single(x => x.Name == "Kia Motors Corporation").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Kia Motors Corporation").Id},
                new VehicleModel{ Name="XCeed", Abrv = makeList.Single(x => x.Name == "Kia Motors Corporation").Abrv,
                    VehicleMakeId = makeList.Single(x => x.Name == "Kia Motors Corporation").Id},
            };
            modelList.ForEach(model => context.VehicleModels.AddOrUpdate(x => x.Name, model));
            context.SaveChanges();
        }
    }
}
