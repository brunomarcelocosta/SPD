using SPD.Model.Model;

namespace SPD.Repository.Interface.Model
{
    public interface IPacienteRepository : IRepositoryBase<Paciente>
    {
        void UpdatePaciente(Paciente paciente);

        void Insert(Paciente paciente);

        void Delete(Paciente paciente);
    }
}
