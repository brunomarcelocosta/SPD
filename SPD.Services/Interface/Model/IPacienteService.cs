using SPD.Model.Model;
using SPD.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Services.Interface.Model
{
    public interface IPacienteService : IServiceBase<Paciente>
    {
        bool ExistePaciente(Paciente paciente);

        [Transactional]
        bool AdiocinarPaciente(Paciente paciente, Usuario usuario, out string resultado);
    }
}
