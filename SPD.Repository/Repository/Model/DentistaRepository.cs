using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;

namespace SPD.Repository.Repository.Model
{
    public class DentistaRepository : RepositoryBase<Dentista>, IDentistaRepository
    {
        public void UpdateDentista(Dentista dentista, Usuario usuario)
        {
            var dentistaUpdate = GetById(dentista.ID);
            try
            {
                dentistaUpdate.NOME = dentista.NOME;
                dentistaUpdate.CRO = dentista.CRO;
                dentistaUpdate.CLINICA = usuario;
                dentistaUpdate.DT_INSERT = dentista.DT_INSERT;

                UpdateEntity(dentistaUpdate);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert(Dentista dentista)
        {
            try
            {
                AddEntity(dentista);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Dentista dentista)
        {
            try
            {
                DeleteEntity(dentista);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
