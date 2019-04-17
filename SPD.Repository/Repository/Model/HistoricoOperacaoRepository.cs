using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
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

        public void RegistraHistorico(string valor, Usuario usuario, Tipo_Operacao kindtipoOperacao, Tipo_Funcionalidades kindfuncionalidade, params string[] valores)
        {
            var id = (int)Enum.Parse(typeof(Tipo_Operacao), kindtipoOperacao.ToString(), true);  //(int)kindtipoOperacao;

            var tipoOperacao = this._TipoOperacaoRepository.GetById(id);
            var funcionalidade = this._FuncionalidadeRepository.GetById((int)kindfuncionalidade);

            var historicoOperacao = new HistoricoOperacao();

            historicoOperacao.RegistraHistorico(this.Context as Context, valor, usuario, tipoOperacao, funcionalidade, valores);

            this.Add(historicoOperacao);
            this.SaveChanges();
        }

        public void Insert(string valor, Usuario usuario, Tipo_Operacao kindtipoOperacao, Tipo_Funcionalidades kindfuncionalidade, params string[] valores)
        {
            var id = (int)Enum.Parse(typeof(Tipo_Operacao), kindtipoOperacao.ToString(), true);  //(int)kindtipoOperacao;

            var tipoOperacao = this._TipoOperacaoRepository.GetById(id);
            var funcionalidade = this._FuncionalidadeRepository.GetById((int)kindfuncionalidade);

            var historicoOperacao = new HistoricoOperacao();

            historicoOperacao.RegistraHistorico(this.Context as Context, valor, usuario, tipoOperacao, funcionalidade, valores);
            try
            {
                this.AddEntity(historicoOperacao);
                this.SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
