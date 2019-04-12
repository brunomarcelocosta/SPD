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
        private readonly IPacienteRepository _PacienteRepository;
        private readonly IPreConsultaRepository _PreConsultaRepository;
        private readonly IAssinaturaRepository _AssinaturaRepository;
        private readonly IHistoricoAutorizacaoPacienteRepository _HistoricoAutorizacaoPacienteRepository;

        public PreConsultaService(IHistoricoOperacaoRepository historicoOperacaoRepository,
                                  IPacienteRepository pacienteRepository,
                                  IPreConsultaRepository preConsultaRepository,
                                  IAssinaturaRepository assinaturaRepository,
                                  IHistoricoAutorizacaoPacienteRepository historicoAutorizacaoPacienteRepository)
            : base(preConsultaRepository)
        {
            _PacienteRepository = pacienteRepository;
            _PreConsultaRepository = preConsultaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _AssinaturaRepository = assinaturaRepository;
            _HistoricoAutorizacaoPacienteRepository = historicoAutorizacaoPacienteRepository;
        }

        public bool ExistePreConsulta(PreConsulta preConsulta)
        {
            var list = _PreConsultaRepository.Query().Where(a => a.ID_PACIENTE == preConsulta.ID_PACIENTE).ToList();

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
                    resultado = "Já existe uma Pré Consulta para este paciente.";
                    return false;
                }

                var paciente = _PacienteRepository.GetById(preConsulta.PACIENTE.ID);

                if (preConsulta.Assinatura != null)
                {
                    var assinatura = _AssinaturaRepository.GetAssinatura(preConsulta.Assinatura, ExisteAssinatura(preConsulta.Assinatura));

                    var historicoAssinatura = new HistoricoAutorizacaoPaciente()
                    {
                        PACIENTE = paciente,
                        ASSINATURA = assinatura,
                        DT_INSERT = DateTime.Now
                    };

                    _HistoricoAutorizacaoPacienteRepository.InsertHistorico(historicoAssinatura);
                }

                preConsulta.PACIENTE = paciente;

                //using (var transactionScope = new TransactionScope())
                //{
                _PreConsultaRepository.Add(preConsulta);
                _PreConsultaRepository.SaveChange();

                _HistoricoOperacaoRepository.RegistraHistorico($"Adicionou a Pré Consulta ao paciente {preConsulta.PACIENTE.NOME}", usuario, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.PreConsulta);
                _HistoricoOperacaoRepository.SaveChanges();


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
                var paciente = _PacienteRepository.GetById(preConsulta.PACIENTE.ID);
                preConsulta.DT_INSERT = DateTime.Now;

                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    _PreConsultaRepository.UpdatePreConsulta(preConsulta, paciente);

                    _HistoricoOperacaoRepository.RegistraHistorico($"Atualizou a Pré Consulta do paciente {preConsulta.PACIENTE.NOME}", usuario, Tipo_Operacao.Alteracao, Tipo_Funcionalidades.PreConsulta);

                    SaveChanges(transactionScope);
                }
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

                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    _PreConsultaRepository.Remove(preConsulta);

                    _HistoricoOperacaoRepository.RegistraHistorico($"Cancelou a Pré Consulta do paciente {preConsulta.PACIENTE.NOME}", usuario, Tipo_Operacao.Exclusao, Tipo_Funcionalidades.PreConsulta);

                    SaveChanges(transactionScope);
                }

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }

        private Assinatura GetAssinatura(Assinatura assinatura)
        {
            var assinaturaReturn = _AssinaturaRepository.GetAssinatura(assinatura, ExisteAssinatura(assinatura));

            return assinaturaReturn;
        }
    }
}
