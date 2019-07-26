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

        public bool UpdateAgenda(int id_agenda, int id_paciente, string nome, string celular, out string resultado)
        {
            resultado = "";

            try
            {
                var agendaUpdate = GetById(id_agenda);

                agendaUpdate.ID_PACIENTE = id_paciente;
                agendaUpdate.NOME_PACIENTE = nome;
                agendaUpdate.CELULAR = celular;

                UpdateEntity(agendaUpdate);

                SaveChange();

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }
    }
}
