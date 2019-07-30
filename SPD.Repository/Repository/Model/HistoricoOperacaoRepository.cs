using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Repository.Repository.Model
{
    public class HistoricoOperacaoRepository : RepositoryBase<HistoricoOperacao>, IHistoricoOperacaoRepository
    {
        private readonly ITipoOperacaoRepository _TipoOperacaoRepository;
        private readonly IFuncionalidadeRepository _FuncionalidadeRepository;

        public HistoricoOperacaoRepository(ITipoOperacaoRepository tipoOperacaoRepository, IFuncionalidadeRepository funcionalidadeRepository)
        {
            _TipoOperacaoRepository = tipoOperacaoRepository;
            _FuncionalidadeRepository = funcionalidadeRepository;
        }

        public void Insert(string valor, Usuario usuario, Tipo_Operacao kindtipoOperacao, Tipo_Funcionalidades kindfuncionalidade, params string[] valores)
        {
            var historicoOperacao = new HistoricoOperacao();

            var id_operacao = (int)Enum.Parse(typeof(Tipo_Operacao), kindtipoOperacao.ToString(), true);
            var id_funcionalidade = (int)kindfuncionalidade;
            var ip = historicoOperacao.ReturnIP(this.Context);
            ip = string.IsNullOrWhiteSpace(ip) ? "" : ip;

            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString()))
            {
                conn.Open();

                var cmd = new SqlCommand("SP_INSERT_HISTORICO_OPERACAO", conn)
                {
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.Add(new SqlParameter("@ip", ip));
                cmd.Parameters.Add(new SqlParameter("@descricao", valor));
                cmd.Parameters.Add(new SqlParameter("@fk_id_usuario", usuario.ID));
                cmd.Parameters.Add(new SqlParameter("@fk_id_tipo_operacao", id_operacao));
                cmd.Parameters.Add(new SqlParameter("@fk_id_funcionalidade", id_funcionalidade));

                cmd.CommandTimeout = 0;

                cmd.ExecuteNonQuery();
            }

        }

        public void Delete(Usuario usuario)
        {
            try
            {
                var historico = Query().Where(a => a.ID_USUARIO == usuario.ID).ToList();

                RemoveEntityRange(historico);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertHistoricoSistema(string valor)
        {
            try
            {
                var historicoOperacao = new HistoricoOperacao();

                historicoOperacao.RegistraHistoricoSistema(valor);

                AddEntity(historicoOperacao);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
