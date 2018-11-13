using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.ViewModels;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class AlterarSenhaController : MapperController<IUsuarioService, Usuario, UsuarioViewModel>
    {
        private readonly IUsuarioService _UsuarioService;

        public AlterarSenhaController(IUsuarioService usuarioService)
            : base(usuarioService)
        {
            this._UsuarioService = usuarioService;
        }

        /// <summary>
        /// Método responsável por renderizar a view da tela de alteração da senha do usuário
        /// </summary>
        /// <returns>view</returns>
        public ActionResult Alterar()
        {
            // Instancia a variável do resultado final do método
            var usuario = this.ApplicationService.GetById(this.GetAuthenticationFromSession().ID);

            if (usuario != null)
            {
                usuario.PASSWORD = "";
            }

            return View(this.ToViewModel(usuario));
        }

        /// <summary>
        /// Método responsável por confirmar alteração da senha do usuário
        /// </summary>
        /// <param name="usuarioViewModel"></param>
        /// <returns>Json: true/false; mensagem caso tenha havido erro</returns>
        [HttpPost]
        public ActionResult Confirmar(UsuarioViewModel usuarioViewModel)
        {
            bool result;
            string Sresult = "";

            // Instancia a variável do resultado final do método
            result = this.ApplicationService.ConfirmarSenha(usuarioViewModel.ID, usuarioViewModel.Password, out Sresult);

            if (result)
            {
                var authenticationViewModel = this.Session[GlobalConstants.Security.AuthenticationSession] as AuthenticationViewModel;

                authenticationViewModel.TrocaSenhaObrigatoria = false;
            }

            return Json(new { Success = result, Mensagem = Sresult });
        }

        /// <summary>
        /// Método responsável por cancelar a alteração da senha do usuário
        /// </summary>
        /// <returns>redirecionamento para método Alterar</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cancelar()
        {
            return this.RedirectToAction("Alterar");
        }
    }
}