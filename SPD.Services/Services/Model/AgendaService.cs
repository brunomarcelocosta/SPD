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
    public class AgendaService : ServiceBase<Agenda>, IAgendaService
    {
        private readonly IAgendaRepository _AgendaRepository;
        private readonly IDentistaRepository _DentistaRepository;
        private readonly IHistoricoOperacaoRepository _HistoricoOperacaoRepository;
        private readonly IPacienteRepository _PacienteRepository;
        private readonly IUsuarioRepository _UsuarioRepository;

        public AgendaService(IAgendaRepository agendaRepository,
                             IDentistaRepository dentistaRepository,
                             IHistoricoOperacaoRepository historicoOperacaoRepository,
                             IPacienteRepository pacienteRepository,
                             IUsuarioRepository usuarioRepository)
            : base(agendaRepository)
        {
            _AgendaRepository = agendaRepository;
            _DentistaRepository = dentistaRepository;
            _HistoricoOperacaoRepository = historicoOperacaoRepository;
            _PacienteRepository = pacienteRepository;
            _UsuarioRepository = usuarioRepository;
        }

        public bool Insert(Agenda agenda, Usuario usuario, out string resultado)
        {
            resultado = "";

            try
            {
                //if (ExistePreConsulta(preConsulta))
                //{
                //    resultado = "Já existe um Pré Atendimento para este paciente.";
                //    return false;
                //}

                var dentista = _DentistaRepository.GetById(agenda.DENTISTA.ID);
                agenda.DENTISTA = dentista;

                var paciente = _PacienteRepository.GetById(agenda.PACIENTE.ID);
                agenda.PACIENTE = paciente;

                //using (TransactionScope transactionScope = Transactional.ExtractTransactional(this.TransactionalMaps))
                //{
                _AgendaRepository.Insert(agenda);

                _HistoricoOperacaoRepository.Insert($"Agendou uma consulta para o paciente {agenda.PACIENTE.NOME}", usuario, Tipo_Operacao.Inclusao, Tipo_Funcionalidades.Agenda);

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
                var agenda = GetById(id);
                agenda.DENTISTA = _DentistaRepository.GetById(agenda.ID_DENTISTA);
                agenda.PACIENTE = agenda.ID_PACIENTE == 0 || agenda.ID_PACIENTE == null ? null : _PacienteRepository.GetById(agenda.ID_PACIENTE.Value);

                var paciente = agenda.NOME_PACIENTE;
                var dentista = agenda.DENTISTA.NOME;

                _AgendaRepository.Delete(agenda);

                _HistoricoOperacaoRepository.Insert($"Excluiu o horário na agenda do Dr(a) {dentista} do paciente {paciente}", usuario, Tipo_Operacao.Exclusao, Tipo_Funcionalidades.Agenda);

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
