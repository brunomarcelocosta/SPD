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
    }
}
