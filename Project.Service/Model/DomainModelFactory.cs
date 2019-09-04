using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Service.Model
{
    public class DomainModelFactory : IDomainModelFactory
    {
        private IVehicleMake _make;
        private IVehicleModel _model;

        public DomainModelFactory(IVehicleMake make, IVehicleModel model)
        {
            this._make = make;
            this._model = model;
        }

        public IVehicleMake MakeInstance()
        {
            return _make;
        }

        public IVehicleModel ModelInstance()
        {
            return _model;
        }
    }
}
