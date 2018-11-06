using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Model
{
    public class TipoOperacao
    {
        public int ID { get; set; } // Alterado para public devido ao mapeamento objeto-relacional do EntityFramework
        public string CODIGO_TIPO_OPERACAO { get; set; }
        public string DESCRICAO_TIPO_OPERACAO { get; set; }

        // Adicionado devido ao mapeamento objeto-relacional do EntityFramework
        public TipoOperacao()
        {
        }

        public TipoOperacao(Enums.SPD_Enums.Tipo_Operacao tipoOperacao)
        {
            this.CODIGO_TIPO_OPERACAO = tipoOperacao.ToString();
            this.DESCRICAO_TIPO_OPERACAO = string.Format(CultureInfo.InvariantCulture, "{0}.", tipoOperacao.ToString());
        }
    }
}
