using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using SPD.Model.Model;
using SPD.Model.Util;
using SPD.MVC.Geral.Content.Texts;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.Geral.ViewModels;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class LoginController : MapperController<IUsuarioService, Usuario, AuthenticationViewModel>
    {
        private readonly IAutenticacaoService _AutenticacaoService;
        private readonly ISessaoUsuarioService _SessaoUsuarioService;
        //private readonly IHistoricoOperacaoService _HistoricoOperacaoService;

        public LoginController(IUsuarioService usuarioService, IAutenticacaoService autenticacaoService,
                               ISessaoUsuarioService sessaoUsuarioService)
            : base(usuarioService)
        {
            _AutenticacaoService = autenticacaoService;
            _SessaoUsuarioService = sessaoUsuarioService;
        }

        [AllowAnonymous]
        public ActionResult AutenticarLogin()
        {
            var authenticationViewModel = this.Session[GlobalConstants.Security.AuthenticationSession] as AuthenticationViewModel;

            if (authenticationViewModel != null)
            {
                this.Session.Remove(GlobalConstants.Security.AuthenticationSession);

                this.Session[GlobalConstants.Security.AuthenticationSession] = null;
            }

            return this.View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> AutenticarLogin(AuthenticationViewModel authenticationViewModel)
        {
            try
            {
                string resultados;
                string enderecoIP = this.Request.UserHostAddress.Equals(GlobalConstants.General.LocalhostIPv6, StringComparison.InvariantCulture) ? GlobalConstants.General.LocalhostIPv4 : this.Request.UserHostAddress;

                Usuario usuario = new Usuario()
                {
                    LOGIN = authenticationViewModel.Login,
                    PASSWORD = authenticationViewModel.Password,
                };

                if (this._AutenticacaoService.AutenticarUsuario(ref usuario, enderecoIP, GlobalConstants.Modules.Web.Url, out resultados))
                {
                    authenticationViewModel.ID = usuario.ID;
                    authenticationViewModel.Nome = usuario.NOME;
                    authenticationViewModel.TrocaSenhaObrigatoria = usuario.TROCA_SENHA_OBRIGATORIA;
                    authenticationViewModel.EnderecoIP = enderecoIP;
                    authenticationViewModel.SessionID = this._SessaoUsuarioService.GetSessaoByUsuarioID(usuario.ID).ID;

                    foreach (var item in usuario.ListUsuarioFuncionalidade)
                    {
                        authenticationViewModel.FuncionalidadesUsuarioIDs.Add(item.ID_FUNCIONALIDADE);
                        authenticationViewModel.FuncionalidadesUsuarioNomes.Add(item.FUNCIONALIDADE.NOME);
                    }

                    this.StoreAuthenticationInSession(authenticationViewModel);

                    var identity = await UsuarioManager.CreateIdentityAsync(usuario, DefaultAuthenticationTypes.ApplicationCookie);
                    identity.AddClaim(new Claim(ClaimTypes.Sid, usuario.ID.ToString()));

                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                    foreach (UsuarioFuncionalidade funcionalidadesUsuario in usuario.ListUsuarioFuncionalidade)
                    {
                        Funcionalidade funcionalidade = funcionalidadesUsuario.FUNCIONALIDADE;

                        if (!funcionalidade.IsATIVO) { continue; }
                        string jsonClaim = new JavaScriptSerializer().Serialize(new UseAuthorization.FuncionalidadeClaim(funcionalidadesUsuario));
                        identity.AddClaim(new Claim(ClaimTypes.Role, jsonClaim));
                    }


                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);

                    return this.RedirectToAction(GlobalConstants.Login.AuthenticatedAction);
                }
                else
                {
                    this.ModelState.Clear();

                    this.ModelState.AddModelError(this.Request.CurrentExecutionFilePath, resultados);
                }
            }
            catch (DataException)
            {
                this.ModelState.AddModelError(this.Request.CurrentExecutionFilePath, String.Format(CultureInfo.InvariantCulture, ErrorResource.CreateEntityMessage, authenticationViewModel.ID));
            }

            return View(authenticationViewModel);
        }

        [HttpPost]
        [AllowAnonymous]
        public JsonResult RedefinePassword(string Login)
        {
            string enderecoIP = this.Request.UserHostAddress.Equals(GlobalConstants.General.LocalhostIPv6, StringComparison.InvariantCulture) ? GlobalConstants.General.LocalhostIPv4 : this.Request.UserHostAddress;

            var emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
            var pwdFrom = ConfigurationManager.AppSettings["pwdFrom"].ToString();

            int Resultado = this.ApplicationService.RedefinirSenha(Login, emailFrom, pwdFrom, enderecoIP);

            if (Resultado > 0)
            {
                if (Resultado == 1)
                {
                    return Json(new { result = "1" }, "application/json");
                }

                if (Resultado == 2)
                {
                    return Json(new { result = "2" }, "application/json");
                }
            }

            return Json(new { result = "0" }, "application/json");
        }

        public ActionResult LoginAutenticado()
        {
            return this.RedirectToAction(GlobalConstants.Home.HomeAction, GlobalConstants.Home.Controller);
        }

        public ActionResult InvalidarLogin()
        {
            return this.RedirectToAction(GlobalConstants.Login.AuthenticateAction);
        }

        [HttpPost]
        public ActionResult InvalidarLogin(AuthenticationViewModel authenticationViewModel)
        {
            try
            {
                int idUsuarioSessaoBanco = this.GetAuthenticationFromSession().ID;

                this.RemoveAuthenticationFromSession();

                this._SessaoUsuarioService.EncerrarSessao(idUsuarioSessaoBanco, "Executou o logoff do sistema.");

                var claimsIdentity = User.Identity as ClaimsIdentity;
                var claimNameList = claimsIdentity.Claims.Select(x => x.Type).ToList();

                while (claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role).Any())
                {
                    claimsIdentity.RemoveClaim(claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role));
                }

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                return this.RedirectToAction(GlobalConstants.Login.AuthenticatedAction);
            }
            catch (DataException)
            {
                this.ModelState.AddModelError(this.Request.CurrentExecutionFilePath, String.Format(CultureInfo.InvariantCulture, ErrorResource.CreateEntityMessage, authenticationViewModel.ID));
            }

            return View(authenticationViewModel);
        }

        [AllowAnonymous]
        public ActionResult Logout(string k)
        {
            try
            {
                int idUsuarioSessaoBanco = Convert.ToInt32(Usuario.DecryptID(k));

                this.RemoveAuthenticationFromSession();

                this._SessaoUsuarioService.EncerrarSessao(idUsuarioSessaoBanco, "Executou o logoff do sistema.");

                var claimsIdentity = User.Identity as ClaimsIdentity;
                var claimNameList = claimsIdentity.Claims.Select(x => x.Type).ToList();

                while (claimsIdentity.Claims.Where(x => x.Type == ClaimTypes.Role).Any())
                {
                    claimsIdentity.RemoveClaim(claimsIdentity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role));
                }

                AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                return View();
            }
            catch (Exception ex)
            {
                //_SysLogApplicationService.Registrar(ex.Message + ex.StackTrace);
                return View("LogoutError");
            }

        }

    }
}