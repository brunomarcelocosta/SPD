using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Services.Services.Model
{
    public class EstadoCivilService : ServiceBase<EstadoCivil>, IEstadoCivilService
    {
        private readonly IEstadoCivilRepository _EstadoCivilRepository;

        public EstadoCivilService(IEstadoCivilRepository estadoCivilRepository)
            : base(estadoCivilRepository)
        {
            _EstadoCivilRepository = estadoCivilRepository;
        }
    }
}
