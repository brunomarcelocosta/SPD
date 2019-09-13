using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Services.Services.Model
{
    public class ConsultaService : ServiceBase<Consulta>, IConsultaService
    {
        private readonly IConsultaRepository _ConsultaRepository;
        private readonly IDentistaRepository _DentistaRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IHistoricoConsultaRepository _HistoricoConsultaRepository;
        private readonly IPreConsultaRepository _PreConsultaRepository;

        public ConsultaService(IConsultaRepository consultaRepository,
                               IDentistaRepository dentistaRepository,
                               IHistoricoOperacaoRepository historicoOperacaoRepository,
                               IHistoricoConsultaRepository historicoConsultaRepository,
                               IPreConsultaRepository preConsultaRepository)
            : base(consultaRepository)
        {
            _ConsultaRepository = consultaRepository;
            _DentistaRepository = dentistaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _HistoricoConsultaRepository = historicoConsultaRepository;
            _PreConsultaRepository = preConsultaRepository;

        }

        public bool Insert(Consulta consulta, string paciente, Usuario usuario, out string resultado)
        {
            resultado = "";

            try
            {
                // consulta.DENTISTA = _DentistaRepository.QueryAsNoTracking().Where(a => a.ID == consulta.DENTISTA.ID).FirstOrDefault();
                // consulta.PRE_CONSULTA = _PreConsultaRepository.QueryAsNoTracking().Where(a => a.ID == consulta.PRE_CONSULTA.ID).FirstOrDefault();

                _ConsultaRepository.InsertSQL(consulta);

                _HistoricoOperacaoRepository.Insert($"Finalizado o atendimento no paciente {paciente}", usuario, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Consulta);

                //var id_consulta = _ConsultaRepository.Query().Where(a => a.ID_PRE_CONSULTA == consulta.PRE_CONSULTA.ID).FirstOrDefault().ID;

                //_HistoricoConsultaRepository.InsertHistorico(id_consulta);

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }
    }
}
