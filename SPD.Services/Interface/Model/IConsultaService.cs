using SPD.Model.Model;

namespace SPD.Services.Interface.Model
{
    public interface IConsultaService : IServiceBase<Consulta>
    {
        bool Insert(Consulta consulta, string paciente, Usuario usuario, out string resultado);
    }
}
