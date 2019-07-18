using SPD.Model.Model;

namespace SPD.Services.Interface.Model
{
    public interface IAgendaService : IServiceBase<Agenda>
    {
        bool Insert(Agenda preConsulta, Usuario usuario, out string resultado);

        bool Delete(int id, Usuario usuario, out string resultado);
    }
}
