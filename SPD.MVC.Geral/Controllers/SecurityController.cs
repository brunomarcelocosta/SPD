using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SPD.CrossCutting.Util;
using SPD.Model.Model;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.Geral.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SPD.MVC.Geral.Controllers
{
    [UseAuthentication]
    public class SecurityController : BaseController
    {
        private readonly IAutenticacaoService _AutenticacaoService;
        private readonly IUsuarioService _UsuarioService;
        private readonly ISessaoUsuarioService _SessaoUsuarioService;

        protected UserManager<Usuario> UsuarioManager { get; private set; }

        /// <summary>
        /// Método responsável por instanciar controle de usuário e sessão
        /// </summary>
        public SecurityController() : base()
        {
            this.UsuarioManager = new UserManager<Usuario>(new UsuarioStore<Usuario>());
            this._AutenticacaoService = IoCServer.GetWebInstance<IAutenticacaoService>();
            this._UsuarioService = IoCServer.GetWebInstance<IUsuarioService>();
            this._SessaoUsuarioService = IoCServer.GetWebInstance<ISessaoUsuarioService>();
        }

        /// <summary>
        /// Método responsável por verificar se usuário está autenticado no sistem
        /// </summary>
        /// <returns>Usuário autenticado ou não autenticado</returns>
        public virtual bool IsAuthenticated()
        {
            var authenticationViewModel = this.Session[GlobalConstants.Security.AuthenticationSession] as AuthenticationViewModel;

            // Verifico no banco apenas se a sessão estiver ativa, pois método é chamado também no login
            if (authenticationViewModel != null)
            {
                var sessaoUsuario = this._SessaoUsuarioService.GetSessaoByUsuarioID(authenticationViewModel.ID);

                if (!this._SessaoUsuarioService.UsuarioConectado(authenticationViewModel.ID) || (sessaoUsuario != null && (!authenticationViewModel.EnderecoIP.Equals(sessaoUsuario.EnderecoIP, StringComparison.InvariantCulture))) || (sessaoUsuario != null && (authenticationViewModel.SessionID != sessaoUsuario.ID)))
                {
                    return false;
                }

                var usuarioFincionalidades = new List<UsuarioFuncionalidade>();

                foreach (var item in this._UsuarioService.GetById(authenticationViewModel.ID).Funcionalidades)
                {
                    usuarioFincionalidades.AddRange(item.UsuarioFuncionalidade);
                }

                // Usado para exibir ou não exibir os botões de ação para as views
                this.ViewBag.FuncoesPerfil = usuarioFincionalidades.AsEnumerable();
            }

            this.ViewBag.Authentication = authenticationViewModel;

            return this.ViewBag.Authentication != null;
        }

        /// <summary>
        /// Método responsável por indicar que usuário não possui permissão para acessar determinada rotina
        /// </summary>
        /// <returns></returns>
        public ActionResult RedirectToNotAuthorized()
        {
            return Content("Não autorizado.");
        }

        public ActionResult RedirectToProcessamentoEmExecucao()
        {
            return View("ProcessamentoEmExecucao");
        }

        public ActionResult ReturnJSON(bool success, string resultado)
        {
            return Json(new { Success = success, Response = resultado });
        }


        /// <summary>
        /// Método responsável por gravar autenticação do usuário na sessão do sistema
        /// </summary>
        /// <param name="authentication"></param>
        public void StoreAuthenticationInSession(AuthenticationViewModel authentication)
        {
            authentication.DataAcesso = DateTime.UtcNow;
            authentication.Password = String.Empty;

            this.Session[GlobalConstants.Security.AuthenticationSession] = authentication;
        }

        /// <summary>
        /// Método responsável por obter autenticação do usuário da sessão do sistema
        /// </summary>
        /// <returns>dados do usuário autenticado</returns>
        public AuthenticationViewModel GetAuthenticationFromSession()
        {
            if (this.IsAuthenticated())
            {
                return this.Session[GlobalConstants.Security.AuthenticationSession] as AuthenticationViewModel;
            }

            return new AuthenticationViewModel() { Nome = null, DataAcesso = null };
        }

        /// <summary>
        /// Método responsável por remover autenticação do usuário da sessão do sistema
        /// </summary>
        public void RemoveAuthenticationFromSession()
        {
            if (this.IsAuthenticated())
            {
                this.Session.Remove(GlobalConstants.Security.AuthenticationSession);

                this.Session[GlobalConstants.Security.AuthenticationSession] = null;
            }
        }

        /// <summary>
        /// Método responsável por abandonar sessão do sistema
        /// </summary>
        public void AbandonAuthentication()
        {
            if (this.IsAuthenticated())
            {
                this.Session.Abandon();
            }
        }

        /// <summary>
        /// Método responsável por renderizar em tela dados da sessão do usuário
        /// </summary>
        /// <returns>PartialView</returns>
        [AllowAnonymous]
        public ActionResult Status()
        {
            return this.PartialView(GlobalConstants.Login.AuthenticationStatusAction, this.GetAuthenticationFromSession());
        }

        /// <summary>
        /// Método responsável por retornar contexto da autenticação
        /// </summary>
        protected IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}
