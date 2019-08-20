using SPD.Model.Model;
using SPD.Model.Util;

namespace SPD.Services.Interface.Model
{
    public interface IPreConsultaService : IServiceBase<PreConsulta>
    {
        bool ExistePreConsulta(PreConsulta preConsulta);

        //[Transactional]
        bool Insert(PreConsulta preConsulta, Usuario usuario, out string resultado);

        //[Transactional]
        bool Delete(int id, Usuario usuario, out string resultado);

        bool Update(PreConsulta preConsulta, Usuario usuario, out string resultado);
    }
}
