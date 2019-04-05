using SPD.Model.Model;
using SPD.Model.Util;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Linq;
using System.Transactions;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.Services.Services.Model
{
    public class PacienteService : ServiceBase<Paciente>, IPacienteService
    {
        private readonly IPacienteRepository _PacienteRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;

        public PacienteService(IPacienteRepository pacienteRepository,
                               IHistoricoOperacaoRepository historicoOperacaoRepository)
            : base(pacienteRepository)
        {
            _PacienteRepository = pacienteRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
        }

        public bool ExistePaciente(Paciente paciente)
        {
            var list = _PacienteRepository.Query().Where(a => a.CPF.Equals(paciente.CPF)).ToList();

            if (list.Count > 0)
            {
                return true;
            }
            else
            {
                list = _PacienteRepository.Query().Where(a => a.NOME.Equals(paciente.NOME)).ToList();

                if (list.Count > 0)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Insert(Paciente paciente, Usuario usuario, out string resultado)
        {
            resultado = "";

            try
            {
                if (ExistePaciente(paciente))
                {
                    resultado = "Já existe paciente cadastrado com este CPF ou com este nome.";
                    return false;
                }

                _PacienteRepository.Add(paciente);
                _PacienteRepository.SaveChanges();

                _HistoricoOperacaoRepository.RegistraHistorico($"Adicionou o paciente {paciente.NOME}", usuario, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Pacientes);
                _HistoricoOperacaoRepository.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                resultado = ex.Message;
                return false;
            }
        }

        public bool Update(Paciente paciente, Usuario usuario, out string resultado)
        {
            resultado = "";

            var pacienteBD = _PacienteRepository.Query().Where(a => a.CPF.Equals(paciente.CPF) || a.NOME.Equals(paciente.NOME)).FirstOrDefault();
            if (pacienteBD.ID != paciente.ID)
            {
                resultado = "Já existe paciente cadastrado com este CPF ou com este Nome.";
                return false;
            }

            try
            {
                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    _PacienteRepository.UpdatePaciente(paciente);

                    _HistoricoOperacaoRepository.RegistraHistorico($"Atualizou o paciente {paciente.NOME}", usuario, Tipo_Operacao.Alteracao, Tipo_Funcionalidades.Pacientes);

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
                var paciente = GetById(id);

                using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                {
                    _PacienteRepository.Remove(paciente);

                    _HistoricoOperacaoRepository.RegistraHistorico($"Inativou o paciente {paciente.NOME}", usuario, Tipo_Operacao.Alteracao, Tipo_Funcionalidades.Pacientes);

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
