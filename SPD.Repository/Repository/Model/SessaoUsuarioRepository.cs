using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace SPD.Repository.Repository.Model
{
    public class SessaoUsuarioRepository : RepositoryBase<SessaoUsuario>, ISessaoUsuarioRepository
    {
        public void CriarSessao(Usuario usuario, string enderecoIP)
        {
            try
            {
                var sessaoUsuario = new SessaoUsuario(usuario, enderecoIP);

                sessaoUsuario.CriarSessao(this.Context as Context);

                this.AddEntity(sessaoUsuario);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool EncerrarSessao(SessaoUsuario sessaoUsuario)
        {
            try
            {
                this.DeleteEntity(sessaoUsuario);

                SaveChange();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void EncerrarSessoes(List<SessaoUsuario> sessaoUsuario, Usuario usuario)
        {
            foreach (var item in sessaoUsuario)
            {
                var todelete = QueryAsNoTracking().Where(a => a.ID == item.ID).ToList();
                RemoveEntityRange(todelete);

                //item.usuario = usuario;
                //base.DeleteEntity(item);

                SaveChange();
            }
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

        public void DeleteSessao(int id_usuario)
        {
            var teste = ConfigurationManager.ConnectionStrings["SqlServer"].ToString();
            //"Data Source = SQL5023.site4now.net; Initial Catalog = DB_A41F74_bmcosta; User Id = DB_A41F74_bmcosta_admin; Password = bruninho02; MultipleActiveResultSets = True; App = EntityFramework;";

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString()))
            {
                conn.Open();

                var delete = $"delete from SPD_SESSAO_USUARIO where fk_id_usuario = {id_usuario}";

                var cmd = new SqlCommand(delete, conn)
                {
                    CommandType = CommandType.Text
                };

                cmd.CommandTimeout = 0;

                cmd.ExecuteNonQuery();
            }
        }

        public void InsertSessao(int id_usuario, string ip)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString()))
            {
                conn.Open();

                var insert = $"insert into SPD_SESSAO_USUARIO(data_hora_acesso, vl_endereco_ip, fk_id_usuario) values(getdate(), '{ip}' ,{id_usuario})";

                var cmd = new SqlCommand(insert, conn)
                {
                    CommandType = CommandType.Text
                };

                cmd.CommandTimeout = 0;

                cmd.ExecuteNonQuery();
            }
        }

    }
}
