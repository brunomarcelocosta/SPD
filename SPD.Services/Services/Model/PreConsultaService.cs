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

        public PreConsultaService(IHistoricoOperacaoRepository historicoOperacaoRepository,
                                  IPacienteRepository pacienteRepository,
                                  IPreConsultaRepository preConsultaRepository)
            : base(preConsultaRepository)
        {
            _PacienteRepository = pacienteRepository;
            _PreConsultaRepository = preConsultaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
        }

        public bool ExistePreConsulta(PreConsulta preConsulta)
        {
            var list = _PreConsultaRepository.Query().Where(a => a.PACIENTE.Equals(preConsulta.PACIENTE.NOME)).ToList();

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
                    resultado = "Já existe paciente com este nome em uma Pré Consulta iniciada.";
                    return false; 
                }

                var paciente = _PacienteRepository.GetById(preConsulta.PACIENTE.ID);
                preConsulta.PACIENTE = paciente;
                preConsulta.DT_INSERT = DateTime.Now;


                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {

                    _PreConsultaRepository.Add(preConsulta);

                    _HistoricoOperacaoRepository.RegistraHistorico($"Iniciou a Pré Consulta do paciente {preConsulta.PACIENTE.NOME}", usuario, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.PreConsulta);

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
    }
}
