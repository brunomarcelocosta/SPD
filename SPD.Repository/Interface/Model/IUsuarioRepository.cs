using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Interface.Model
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        bool Bloquear(Usuario usuario);

        bool EmailJaCadastrado(String email);

        void Atualizar(Usuario usuario);

        bool NomeJaUtilizado(string nome);

        Usuario GetByLoginSenha(string login, string password);

        Usuario GetByLogin(string login);

        bool DesBloquear(Usuario usuario);

        bool IsAtivo(Usuario usuario);

        bool IsBloqueado(Usuario usuario);

        Usuario ZerarTentativas(Usuario usuario);

        Usuario IncrementarTentativas(Usuario usuario);
    }
}
