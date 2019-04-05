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

        bool Insert(Paciente paciente, Usuario usuario, out string resultado);

        [Transactional]
        bool Update(Paciente paciente, Usuario usuario, out string resultado);

        [Transactional]
        bool Delete(int id, Usuario usuario, out string resultado);
    }
}
