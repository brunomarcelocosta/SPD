using SPD.Model.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Repository.Interface.Model
{
    public interface IHistoricoOperacaoRepository : IRepositoryBase<HistoricoOperacao>
    {
        void RegistraHistorico(string valor, Usuario usuario, Tipo_Operacao kindtipoOperacao, Tipo_Funcionalidades kindfuncionalidade, params string[] valores);

        void Insert(string valor, Usuario usuario, Tipo_Operacao kindtipoOperacao, Tipo_Funcionalidades kindfuncionalidade, params string[] valores);
    }
}
