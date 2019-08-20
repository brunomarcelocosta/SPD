using SPD.Model.Model;
using SPD.Model.Util;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Services.Services.Model
{
    public class PreConsultaService : ServiceBase<PreConsulta>, IPreConsultaService
    {
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IAgendaRepository _AgendaRepository;
        private readonly IPreConsultaRepository _PreConsultaRepository;
        private readonly IAssinaturaRepository _AssinaturaRepository;
        private readonly IHistoricoAutorizacaoPacienteRepository _HistoricoAutorizacaoPacienteRepository;
        private readonly IPacienteRepository _PacienteRepository;

        public PreConsultaService(IHistoricoOperacaoRepository historicoOperacaoRepository,
                                  IAgendaRepository agendaRepository,
                                  IPreConsultaRepository preConsultaRepository,
                                  IAssinaturaRepository assinaturaRepository,
                                  IPacienteRepository pacienteRepository,
                                  IHistoricoAutorizacaoPacienteRepository historicoAutorizacaoPacienteRepository)
            : base(preConsultaRepository)
        {
            _AgendaRepository = agendaRepository;
            _PreConsultaRepository = preConsultaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _AssinaturaRepository = assinaturaRepository;
            _PacienteRepository = pacienteRepository;
            _HistoricoAutorizacaoPacienteRepository = historicoAutorizacaoPacienteRepository;
        }

        public bool ExistePreConsulta(PreConsulta preConsulta)
        {
            var list = _PreConsultaRepository.Query().Where(a => a.ID_AGENDA == preConsulta.ID_AGENDA).ToList();

            if (list.Count > 0)
                return true;

            return false;
        }

        public bool ExisteAssinatura(Assinatura assinatura)
        {
            var list = _AssinaturaRepository.Query().Where(a => a.CPF_RESPONSAVEL == assinatura.CPF_RESPONSAVEL).ToList();

            if (list.Count > 0)
                return true;

            return false;
        }

        public bool Insert(PreConsulta preConsulta, Usuario usuario, out string resultado)
        {
            resultado = "";

            try
            {
                if (ExistePreConsulta(preConsulta))
                {
                    resultado = "Já existe um Pré Atendimento para este paciente.";
                    return false;
                }

                var agenda = _AgendaRepository.QueryAsNoTracking().Where(a => a.ID == preConsulta.AGENDA.ID).FirstOrDefault();
                preConsulta.AGENDA = agenda;

                var paciente = _PacienteRepository.QueryAsNoTracking().Where(a => a.ID == agenda.ID_PACIENTE.Value).FirstOrDefault();

                if (preConsulta.Assinatura != null)
                {
                    //var assinatura = _AssinaturaRepository.GetAssinatura(preConsulta.Assinatura, ExisteAssinatura(preConsulta.Assinatura));

                    _HistoricoAutorizacaoPacienteRepository
                    .InsertHistorico(paciente.ID, preConsulta.Assinatura.ASSINATURA, preConsulta.Assinatura.NOME_RESPONSAVEL, preConsulta.Assinatura.CPF_RESPONSAVEL, out int id_assinatura);

                    preConsulta.ID_ASSINATURA = id_assinatura;
                }

                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                _PreConsultaRepository.Insert(preConsulta);

                _HistoricoOperacaoRepository.Insert($"Adicionou o Pré Atendimento ao paciente {preConsulta.AGENDA.NOME_PACIENTE}", usuario, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.PreConsulta);

                //    SaveChanges(transactionScope);
                //}

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }

        public bool Delete(int id, Usuario usuario, out string resultado)
        {
            resultado = "";

            try
            {
                var preConsulta = GetById(id);
                preConsulta.AGENDA = _AgendaRepository.GetById(preConsulta.ID_AGENDA);

                var paciente = _PacienteRepository.GetById(preConsulta.AGENDA.ID_PACIENTE.Value);

                var nome = paciente.NOME;

                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                _PreConsultaRepository.Delete(preConsulta);

                _HistoricoOperacaoRepository.Insert($"Excluiu o Pré Atendimento do paciente {nome}", usuario, Tipo_Operacao.Exclusao, Tipo_Funcionalidades.PreConsulta);

                //SaveChanges(transactionScope);
                //}

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }

        public bool Update(PreConsulta preConsulta, Usuario usuario, out string resultado)
        {
            resultado = "";

            try
            {

                var agenda = _AgendaRepository.Query().Where(a => a.ID == preConsulta.AGENDA.ID).FirstOrDefault();
                //preConsulta.AGENDA = agenda;


                _PreConsultaRepository.UpdatePreConsulta(preConsulta, agenda);

                _HistoricoOperacaoRepository.Insert($"Atualizou o Pré Atendimento ao paciente {preConsulta.AGENDA.NOME_PACIENTE}", usuario, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.PreConsulta);

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
