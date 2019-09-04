using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.MVC.Models.Factories
{
    public class DTOFactory : IDTOFactory
    {
        private VehicleMakeDTO _makeDTO;
        private VehicleModelDTO _modelDTO;

        public DTOFactory(VehicleMakeDTO makeDTO, VehicleModelDTO modelDTO)
        {
            _makeDTO = makeDTO;
            _modelDTO = modelDTO;
        }

        public VehicleMakeDTO MakeDTOInstance()
        {
            return _makeDTO;
        }

        public VehicleModelDTO ModelDTOInstance()
        {
            return _modelDTO;
        }
    }
}