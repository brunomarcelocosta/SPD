using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;

namespace SPD.Repository.Repository.Model
{
    public class AgendaRepository : RepositoryBase<Agenda>, IAgendaRepository
    {
        public void Insert(Agenda agenda)
        {
            try
            {
                AddEntity(agenda);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Agenda agenda)
        {
            try
            {
                base.DeleteEntity(agenda);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateAgenda(int id_agenda, int id_paciente, string nome_paciente)
        {
            try
            {
                var agendaToUpdate = GetById(id_agenda);

                agendaToUpdate.ID_PACIENTE = id_paciente;
                agendaToUpdate.NOME_PACIENTE = nome_paciente;

                UpdateEntity(agendaToUpdate);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
