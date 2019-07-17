using SimpleInjector;
using SPD.Repository.Interface;
using SPD.Repository.Interface.Model;
using SPD.Repository.Repository;
using SPD.Repository.Repository.Model;
using SPD.Services.Interface;
using SPD.Services.Interface.Model;
using SPD.Services.Services;
using SPD.Services.Services.Model;

namespace SPD.CrossCutting
{
    public class BootStrapper
    {
        public static void RegisterServices(Container container)
        {

            #region Service

            container.Register(typeof(IServiceBase<>), typeof(ServiceBase<>), Lifestyle.Scoped);
            container.Register(typeof(IAgendaService), typeof(AgendaService), Lifestyle.Scoped);
            container.Register(typeof(IAssinaturaService), typeof(AssinaturaService), Lifestyle.Scoped);
            container.Register(typeof(IAutenticacaoService), typeof(AutenticacaoService), Lifestyle.Scoped);
            container.Register(typeof(IConsultaService), typeof(ConsultaService), Lifestyle.Scoped);
            container.Register(typeof(IDentistaService), typeof(DentistaService), Lifestyle.Scoped);
            container.Register(typeof(IFuncionalidadeService), typeof(FuncionalidadeService), Lifestyle.Scoped);
            container.Register(typeof(IHistoricoConsultaService), typeof(HistoricoConsultaService), Lifestyle.Scoped);
            container.Register(typeof(IHistoricoOperacaoService), typeof(HistoricoOperacaoService), Lifestyle.Scoped);
            container.Register(typeof(IPacienteService), typeof(PacienteService), Lifestyle.Scoped);
            container.Register(typeof(IPreConsultaService), typeof(PreConsultaService), Lifestyle.Scoped);
            container.Register(typeof(ISessaoUsuarioService), typeof(SessaoUsuarioService), Lifestyle.Scoped);
            container.Register(typeof(ITipoOperacaoService), typeof(TipoOperacaoService), Lifestyle.Scoped);
            container.Register(typeof(IUsuarioService), typeof(UsuarioService), Lifestyle.Scoped);
            container.Register(typeof(IUsuarioFuncionalidadeService), typeof(UsuarioFuncionalidadeService), Lifestyle.Scoped);

            #endregion

            #region Repository

            container.Register(typeof(IRepositoryBase<>), typeof(RepositoryBase<>), Lifestyle.Scoped);
            container.Register(typeof(IAgendaRepository), typeof(AgendaRepository), Lifestyle.Scoped);
            container.Register(typeof(IAssinaturaRepository), typeof(AssinaturaRepository), Lifestyle.Scoped);
            container.Register(typeof(IConsultaRepository), typeof(ConsultaRepository), Lifestyle.Scoped);
            container.Register(typeof(IDentistaRepository), typeof(DentistaRepository), Lifestyle.Scoped);
            container.Register(typeof(IFuncionalidadeRepository), typeof(FuncionalidadeRepository), Lifestyle.Scoped);
            container.Register(typeof(IHistoricoAutorizacaoPacienteRepository), typeof(HistoricoAutorizacaoPacienteRepository), Lifestyle.Scoped);
            container.Register(typeof(IHistoricoConsultaRepository), typeof(HistoricoConsultaRepository), Lifestyle.Scoped);
            container.Register(typeof(IHistoricoOperacaoRepository), typeof(HistoricoOperacaoRepository), Lifestyle.Scoped);
            container.Register(typeof(INotificacaoRepository), typeof(NotificacaoRepository), Lifestyle.Scoped);
            container.Register(typeof(IPacienteRepository), typeof(PacienteRepository), Lifestyle.Scoped);
            container.Register(typeof(IPreConsultaRepository), typeof(PreConsultaRepository), Lifestyle.Scoped);
            container.Register(typeof(ISessaoUsuarioRepository), typeof(SessaoUsuarioRepository), Lifestyle.Scoped);
            container.Register(typeof(ITipoOperacaoRepository), typeof(TipoOperacaoRepository), Lifestyle.Scoped);
            container.Register(typeof(IUsuarioFuncionalidadeRepository), typeof(UsuarioFuncionalidadeRepository), Lifestyle.Scoped);
            container.Register(typeof(IUsuarioRepository), typeof(UsuarioRepository), Lifestyle.Scoped);


            #endregion

        }
    }
}
