using SPD.Model.Model;
using SPD.Repository.Interface.Model;

namespace SPD.Repository.Repository.Model
{
    public class DentistaRepository : RepositoryBase<Dentista>, IDentistaRepository
    {
        public void UpdateDentista(Dentista dentista)
        {
            var dentistaUpdate = GetById(dentista.ID);

            dentistaUpdate.NOME = dentista.NOME;
            dentistaUpdate.CRO = dentista.CRO;
            dentistaUpdate.USUARIO = dentista.USUARIO;
            dentistaUpdate.DT_INSERT = dentista.DT_INSERT;

            Update(dentistaUpdate);
        }
    }
}
