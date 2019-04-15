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
        void RegistraHistoricoSistema(string valor);

        void RegistraHistorico(string valor, Usuario usuario, Tipo_Operacao kindtipoOperacao, Tipo_Funcionalidades kindfuncionalidade, params string[] valores);

        bool RegistraHistoricoRepository(string valor, Usuario usuario, Tipo_Operacao kindtipoOperacao, Tipo_Funcionalidades kindfuncionalidade, out string resultado);

        void ExcluiHistoricoUsuario(Usuario usuario);

        void RegistraHistoricoSC(string valor, Usuario usuario, Tipo_Operacao kindtipoOperacao, Tipo_Funcionalidades kindfuncionalidade, params string[] valores);
    }
}
