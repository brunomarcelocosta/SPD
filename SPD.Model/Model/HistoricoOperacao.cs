using SPD.Model.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Model.Model
{
    public class HistoricoOperacao
    {
        public int ID { set; get; }

        public string ENDERECO_IP { get; set; }

        public string DESCRICAO { get; set; }

        public string DUMP { get; set; }

        public int? ID_USUARIO { get; set; }
        public virtual Usuario USUARIO { get; set; }

        public int ID_TIPO_OPERACAO { get; set; }
        public virtual TipoOperacao tipoOperacao { get; set; }

        public int? ID_FUNCIONALIDADE { get; set; }
        public virtual Funcionalidade FUNCIONALIDADE { get; set; }

        public DateTime DT_OPERACAO { get; set; }

        private string SessaoUID { get; set; }


        #region métodos

        public HistoricoOperacao()
        {
            this.USUARIO = null;
        }

        public HistoricoOperacao(Usuario usuario)
            : this()
        {
            this.USUARIO = usuario;
        }

        public HistoricoOperacao(Usuario usuario, Funcionalidade funcionalidade)
            : this()
        {
            this.USUARIO = usuario;
            this.FUNCIONALIDADE = FUNCIONALIDADE;
        }

        public void RegistraHistorico(Context context, string valor, Usuario usuario, Tipo_Operacao tipoOperacao, params string[] valores)
        {
            StringBuilder dump = new StringBuilder();

            if (valores != null)
            {
                foreach (var valorDump in valores)
                {
                    if (!string.IsNullOrEmpty(valorDump))
                    {
                        dump.AppendFormat(CultureInfo.InvariantCulture, "{0};", valorDump);
                    }
                }
            }
            else
            {
                dump.AppendFormat(CultureInfo.InvariantCulture, "{0};", "");
            }

            this.ENDERECO_IP = context.usuarioIP;
            this.DESCRICAO = valor;
            this.DT_OPERACAO = DateTime.Now;
            this.DUMP = dump.ToString();
            this.SessaoUID = context.usuarioSessionID;

            //Wendel 10/01/2018 - retirado pois criava um novo registro na tabela de tipo de operação. Atribuído ID na service.
            //this.TipoOperacao = new TipoOperacao(tipoOperacao);

            this.ID_USUARIO = USUARIO.ID;
            // ToDo: Prover a Funcionalidade.
            this.FUNCIONALIDADE = null;
        }

        public void RegistraHistorico(Context context, string valor, Usuario usuario, TipoOperacao tipoOperacao, Funcionalidade funcionalidade, params string[] valores)
        {
            StringBuilder dump = new StringBuilder();

            if (valores != null)
            {
                foreach (var valorDump in valores)
                {
                    if (!string.IsNullOrEmpty(valorDump))
                    {
                        dump.AppendFormat(CultureInfo.InvariantCulture, "{0};", valorDump);
                    }
                }
            }
            else
            {
                dump.AppendFormat(CultureInfo.InvariantCulture, "{0};", "");
            }

            this.ENDERECO_IP = context.usuarioIP;
            this.DESCRICAO = valor;
            this.DT_OPERACAO = DateTime.Now;
            this.DUMP = dump.ToString();
            this.SessaoUID = context.usuarioSessionID;

            this.tipoOperacao = tipoOperacao;
            this.USUARIO = usuario;
            this.FUNCIONALIDADE = funcionalidade;
        }

        public void RegistraHistoricoSistema(string valor)
        {
            this.DESCRICAO = valor;
            this.DT_OPERACAO = DateTime.Now;
            this.tipoOperacao = new TipoOperacao(SPD_Enums.Tipo_Operacao.Sistema);
        }

        public string ReturnIP(Context context, string valores, out string Dump)
        {
            StringBuilder dump = new StringBuilder();

            if (valores != null)
            {
                foreach (var valorDump in valores)
                {
                    dump.AppendFormat(CultureInfo.InvariantCulture, "{0};", valorDump);
                }
            }
            else
            {
                dump.AppendFormat(CultureInfo.InvariantCulture, "{0};", "");
            }

            Dump = dump.ToString();

            return context.usuarioIP;
        }

        #endregion
    }
}
