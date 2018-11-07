using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Global;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SPD.MVC.Geral.Utilities
{
    public sealed class UseAuthorization : ActionFilterAttribute
    {
        public string Funcionalidades { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var securityController = filterContext.Controller as SecurityController;
            var claimsIdentity = filterContext.HttpContext.User.Identity as ClaimsIdentity;
            var funcionalidadesUsuario = GetFuncionalidadesFromClaims(claimsIdentity);

            var funcionalidade = new JavaScriptSerializer().Deserialize<FuncionalidadeClaim>(Funcionalidades);
            var funcionalidadeUsuario = funcionalidadesUsuario.Where(fU => fU.Nome.Equals(funcionalidade.Nome)).FirstOrDefault();

            /*
            if (funcionalidadeUsuario==null || !funcionalidadeUsuario.Has(funcionalidade)){

                filterContext.Result = securityController.RedirectToNotAuthorized();

                if(funcionalidade.Nome == DataConstants.Funcionalidades.TelaInicialSistema)
                {
                    filterContext.Result = new RedirectResult(GlobalConstants.Authentication.AuthenticateUrl);
                }
            }
            */

            if (funcionalidadesUsuario.Count() == 0)
            {
                filterContext.Result = new RedirectResult(GlobalConstants.Login.AuthenticateUrl);
            }
            else if (funcionalidadeUsuario == null || !funcionalidadeUsuario.Has(funcionalidade))
            {
                filterContext.Result = securityController.RedirectToNotAuthorized();
            }

        }

        private IEnumerable<FuncionalidadeClaim> GetFuncionalidadesFromClaims(ClaimsIdentity claimsIdentity)
        {
            ICollection<FuncionalidadeClaim> funcionalidades = new List<FuncionalidadeClaim>();

            foreach (Claim claim in claimsIdentity.Claims)
            {
                if (claim.Type.Equals(ClaimTypes.Role))
                {
                    string json = claim.Value;
                    FuncionalidadeClaim funcionalidadeClaim = new JavaScriptSerializer().Deserialize<FuncionalidadeClaim>(json);
                    funcionalidades.Add(funcionalidadeClaim);
                }
            }

            return funcionalidades;
        }

        public sealed class FuncionalidadeClaim
        {
            public string Nome { get; set; }

            public FuncionalidadeClaim() { }

            public FuncionalidadeClaim(UsuarioFuncionalidade usuarioFuncionalidade)
            {
                Nome = usuarioFuncionalidade.FUNCIONALIDADE.Nome;
            }

            public bool Has(FuncionalidadeClaim funcionalidadeClaim)
            {
                if (!funcionalidadeClaim.Nome.Equals(Nome)) { return false; }

                bool valid = true;
                return valid;
            }
        }
    }

}
