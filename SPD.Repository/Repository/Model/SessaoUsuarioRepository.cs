using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace SPD.Repository.Repository.Model
{
    public class SessaoUsuarioRepository : RepositoryBase<SessaoUsuario>, ISessaoUsuarioRepository
    {
        public void CriarSessao(Usuario usuario, string enderecoIP)
        {
            var sessaoUsuario = new SessaoUsuario(usuario, enderecoIP);

            sessaoUsuario.CriarSessao(this.Context as Context);

            this.Add(sessaoUsuario);
        }

        public bool EncerrarSessao(SessaoUsuario sessaoUsuario)
        {
            try
            {
                this.Remove(sessaoUsuario);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public List<SessaoUsuario> VerificaConexoesSimultaneas(Usuario usuario)
        {
            return this.DomainContext.DataContext.Entity.Set<SessaoUsuario>().Where(sessaoUsuario => sessaoUsuario.ID_USUARIO.Equals(usuario.ID)).ToList();
        }

        public bool UsuarioConectado(int usuarioID)
        {
            return this.DomainContext.DataContext.Entity.Set<SessaoUsuario>().Where(sessaoUsuario => sessaoUsuario.ID_USUARIO.Equals(usuarioID)).ToList().Count > 0;
        }

        public SessaoUsuario GetSessaoByUsuarioID(int usuarioID)
        {
            return this.DomainContext.DataContext.Entity.Set<SessaoUsuario>().Where(sessaoUsuario => sessaoUsuario.usuario.ID == usuarioID).FirstOrDefault();
        }

        public void DesconetarSessaoUsuarios(Usuario usuario, out List<SessaoUsuario> sessoes)
        {
            if (usuario != null)
            {
                sessoes = this.DomainContext.DataContext.Entity.Set<SessaoUsuario>().Where(sessaoUsuario => sessaoUsuario.usuario.ID != usuario.ID).ToList();
                this.DomainContext.DataContext.Entity.Set<SessaoUsuario>().RemoveRange(sessoes);
            }
            else
            {
                sessoes = new List<SessaoUsuario>();
            }
        }

    }
}
