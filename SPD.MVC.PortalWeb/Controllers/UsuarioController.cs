using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class UsuarioController : MapperController<IUsuarioService, Usuario, UsuarioViewModel>
    {
        private readonly IUsuarioService _UsuarioService;
        private readonly IFuncionalidadeService _FuncionalidadeService;
        private readonly IUsuarioFuncionalidadeService _UsuarioFuncionalidadeService;

        public UsuarioController(IUsuarioService usuarioService,
                                 IFuncionalidadeService funcionalidadeService,
                                 IUsuarioFuncionalidadeService usuarioFuncionalidadeService)
            : base(usuarioService)
        {
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuarioFuncionalidadeService;
        }

        #region List

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Usuários\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            usuarioViewModel = ReturnUsuario(collection);

            return View(usuarioViewModel);
        }

        [HttpPost]
        public ActionResult Paginacao(FormCollection collection = null)
        {
            // Initialization
            //string search = Request.Form.GetValues("search[value]")[0];
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            if (pageSize == -1)
            {
                pageSize = 50;
            }

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            usuarioViewModel = ReturnUsuario(collection);

            int totalRecords = usuarioViewModel.ListUsuarioViewModel.Count;

            usuarioViewModel.ListUsuarioViewModel = Ordenacao(order, orderDir, usuarioViewModel.ListUsuarioViewModel);

            int recFilter = usuarioViewModel.ListUsuarioViewModel.Count;

            usuarioViewModel.ListUsuarioViewModel = usuarioViewModel.ListUsuarioViewModel.Skip(startRec).Take(pageSize).ToList();

            List<object> listToView = new List<object>();

            foreach (var item in usuarioViewModel.ListUsuarioViewModel)
            {
                listToView.Add(new
                {
                    item.ID,
                    item.Nome,
                    item.Email,
                    isAtivo = item.isAtivo == true ? "Ativo" : "Inativo",
                    isBloqueado = item.isBloqueado == true ? "Sim" : "Não"
                });
            }

            return Json(new
            {
                draw = Convert.ToInt32(draw),
                recordsTotal = totalRecords,
                recordsFiltered = recFilter,
                data = listToView
            }, JsonRequestBehavior.AllowGet);
        }

        private List<UsuarioViewModel> Ordenacao(string order, string orderDir, List<UsuarioViewModel> data)
        {
            // Initialization
            List<UsuarioViewModel> lst = new List<UsuarioViewModel>();

            try
            {
                // Sorting   
                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Nome).ToList() : data.OrderBy(p => p.Nome).ToList();
                        break;

                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email).ToList() : data.OrderBy(p => p.Email).ToList();
                        break;

                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.isAtivo).ToList() : data.OrderBy(p => p.isAtivo).ToList();
                        break;

                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.isBloqueado).ToList() : data.OrderBy(p => p.isBloqueado).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Nome).ToList() : data.OrderBy(p => p.Nome).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return lst;
        }

        public UsuarioViewModel ReturnUsuario(FormCollection collection = null)
        {
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            usuarioViewModel.ListUsuarioViewModel = ToListViewModel(_UsuarioService.QueryAsNoTracking().ToList());

            usuarioViewModel = Filtrar(usuarioViewModel, collection);

            return usuarioViewModel;
        }

        #endregion

        #region Editar

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Usuários\"}")]
        public ActionResult Edit(int id)
        {
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            usuarioViewModel = ToViewModel(_UsuarioService.GetById(id));

            if (usuarioViewModel == null)
            {
                return Redirect(GlobalConstants.Modules.Action(GlobalConstants.Modules.Web.Url, "Home", "Index"));
            }

            var usuariosFuncionalidades = ToListViewModel<UsuarioFuncionalidade, UsuarioFuncionalidadeViewModel>(_UsuarioFuncionalidadeService.Query().Where(a => a.ID_USUARIO == usuarioViewModel.ID).ToList());

            return View(usuarioViewModel);
        }

        #endregion

        #region Filtrar

        public UsuarioViewModel Filtrar(UsuarioViewModel usuarioViewModel, FormCollection collection)
        {
            List<UsuarioViewModel> ListaFiltrada = new List<UsuarioViewModel>();

            ListaFiltrada = usuarioViewModel.ListUsuarioViewModel;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Nome"]))
                {
                    var nome = collection["Nome"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Nome.Contains(nome)).ToList();
                    usuarioViewModel.Nome = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["Email"]))
                {
                    var email = collection["Email"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Email.Contains(email)).ToList();
                    usuarioViewModel.Nome = email;
                }

                if (!string.IsNullOrWhiteSpace(collection["isAtivo"]))
                {
                    var status = collection["isAtivo"].ToString() == "true" ? true : false;

                    ListaFiltrada = ListaFiltrada.Where(a => a.isAtivo == status).ToList();
                    usuarioViewModel.isAtivo = status;
                }

                if (!string.IsNullOrWhiteSpace(collection["isBloqueado"]))
                {
                    var status = collection["isBloqueado"].ToString() == "true" ? true : false;

                    ListaFiltrada = ListaFiltrada.Where(a => a.isBloqueado == status).ToList();
                    usuarioViewModel.isBloqueado = status;
                }
            }

            usuarioViewModel.ListUsuarioViewModel = ListaFiltrada;

            return usuarioViewModel;
        }

        #endregion
    }
}