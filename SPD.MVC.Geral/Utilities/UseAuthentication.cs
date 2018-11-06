using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Global;
using System;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace SPD.MVC.Geral.Utilities
{
    /// <summary>
    /// Atributo utilizado para verificar autenticação de clientes em acesso a controllers.
    /// </summary>
    public sealed class UseAuthentication : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (!filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                SecurityController securityController = filterContext.Controller as SecurityController;

                if (securityController.IsAuthenticated())
                {
                    if ((securityController.GetAuthenticationFromSession().TrocaSenhaObrigatoria) &&
                        !filterContext.HttpContext.Request.Url.OriginalString.Equals(GlobalConstants.RedefinePassword.RedefinePasswordUrl, StringComparison.InvariantCulture) &&
                        !filterContext.HttpContext.Request.Url.OriginalString.Equals(GlobalConstants.ConfirmRedefinePassword.ConfirmRedefinePasswordUrl, StringComparison.InvariantCulture))
                    {
                        filterContext.Result = new RedirectResult(GlobalConstants.RedefinePassword.RedefinePasswordUrl);
                    }
                }
                else
                {
                    filterContext.Result = new RedirectResult(GlobalConstants.Login.AuthenticateUrl);

                    /* ToDo: Habilite este bloco para roteamento interno.
                    filterContext.Result = new RedirectToRouteResult(
                        new RouteValueDictionary
                        {
                            {
                                GlobalConstants.ControllerKey, GlobalConstants.Authentication.Controller
                            },
                            {
                                GlobalConstants.ActionKey, GlobalConstants.Authentication.AuthenticateAction
                            }
                        }
                    );
                    */
                }
            }
        }
    }
}
