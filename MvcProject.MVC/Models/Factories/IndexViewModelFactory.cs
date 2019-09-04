using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcProject.MVC.Models.Factories
{
    public class IndexViewModelFactory : IIndexViewModelFactory
    {
        private IndexViewModel<VehicleMakeDTO, VehicleModelDTO> _makeIndexViewModel;
        private IndexViewModel<VehicleModelDTO, string> _modelIndexViewModel;

        public IndexViewModelFactory(
            IndexViewModel<VehicleMakeDTO, VehicleModelDTO> makeIndexViewModel,
            IndexViewModel<VehicleModelDTO, string> modelIndexViewModel)
        {
            _makeIndexViewModel = makeIndexViewModel;
            _modelIndexViewModel = modelIndexViewModel;
        }

        public IndexViewModel<VehicleMakeDTO, VehicleModelDTO> MakeIndexViewModelInstance()
        {
            return _makeIndexViewModel;
        }

        public IndexViewModel<VehicleModelDTO, string> ModelIndexViewModelInstance()
        {
            return _modelIndexViewModel;
        }
    }
}