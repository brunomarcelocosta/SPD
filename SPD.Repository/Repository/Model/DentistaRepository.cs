using SPD.Model.Model;
using SPD.Repository.Interface.Model;

namespace SPD.Repository.Repository.Model
{
    public class DentistaRepository : RepositoryBase<Dentista>, IDentistaRepository
    {
        private readonly IUsuarioRepository _UsuarioRepository;

        public void UpdateDentista(Dentista dentista)
        {
            var dentistaUpdate = GetById(dentista.ID);
            var user = _UsuarioRepository.GetById(dentista.ID_USUARIO);

            dentistaUpdate.NOME = dentista.NOME;
            dentistaUpdate.CRO = dentista.CRO;
            dentistaUpdate.USUARIO = user;
            dentistaUpdate.DT_INSERT = dentista.DT_INSERT;

            Update(dentistaUpdate);
        }
    }
}
