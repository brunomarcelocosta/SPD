using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
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
    public class DentistaController : MapperController<IDentistaService, Dentista, DentistaViewModel>
    {

        private readonly IDentistaService _DentistaService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IFuncionalidadeService _FuncionalidadeService;
        private readonly IUsuarioFuncionalidadeService _UsuarioFuncionalidadeService;

        public DentistaController(IDentistaService dentistaService,
                                  IUsuarioService usuarioService,
                                  IFuncionalidadeService funcionalidadeService,
                                  IUsuarioFuncionalidadeService usuarioFuncionalidadeService)
            : base(dentistaService)
        {
            _DentistaService = dentistaService;
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuarioFuncionalidadeService;
        }

        #region List

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Dentistas\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            DentistaViewModel dentistaViewModel = new DentistaViewModel();

            dentistaViewModel = ReturnDentista(collection); ;

            return View(dentistaViewModel);
        }

        [HttpPost]
        public ActionResult Paginacao(FormCollection collection = null)
        {
            string draw = Request.Form.GetValues("draw")[0];
            string order = Request.Form.GetValues("order[0][column]")[0];
            string orderDir = Request.Form.GetValues("order[0][dir]")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            if (pageSize == -1)
            {
                pageSize = 50;
            }

            DentistaViewModel dentistaViewModel = new DentistaViewModel();

            dentistaViewModel = ReturnDentista(collection);

            int totalRecords = dentistaViewModel.ListDentistaViewModel.Count;

            dentistaViewModel.ListDentistaViewModel = Ordenacao(order, orderDir, dentistaViewModel.ListDentistaViewModel);

            int recFilter = dentistaViewModel.ListDentistaViewModel.Count;

            dentistaViewModel.ListDentistaViewModel = dentistaViewModel.ListDentistaViewModel.Skip(startRec).Take(pageSize).ToList();

            List<object> listToView = new List<object>();

            foreach (var item in dentistaViewModel.ListDentistaViewModel)
            {
                listToView.Add(new
                {
                    item.ID,
                    item.Nome,
                    item.Cro,
                    Usuario = item.Usuario.Nome,
                    Criado = item.Dt_Insert
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

        private List<DentistaViewModel> Ordenacao(string order, string orderDir, List<DentistaViewModel> data)
        {
            // Initialization
            List<DentistaViewModel> lst = new List<DentistaViewModel>();

            try
            {

                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ID).ToList() : data.OrderBy(p => p.ID).ToList();
                        break;

                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Nome).ToList() : data.OrderBy(p => p.Nome).ToList();
                        break;

                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cro).ToList() : data.OrderBy(p => p.Cro).ToList();
                        break;

                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Usuario.Nome).ToList() : data.OrderBy(p => p.Usuario.Nome).ToList();
                        break;

                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Dt_Insert).ToList() : data.OrderBy(p => p.Dt_Insert).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ID).ToList() : data.OrderBy(p => p.ID).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return lst;
        }

        public DentistaViewModel ReturnDentista(FormCollection collection = null)
        {
            DentistaViewModel dentistaViewModel = new DentistaViewModel();

            dentistaViewModel.ListDentistaViewModel = ToListViewModel(_DentistaService.QueryAsNoTracking().ToList());

            dentistaViewModel = Filtrar(dentistaViewModel, collection);

            return dentistaViewModel;
        }

        #endregion

        #region Novo

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Adicionar Dentistas\"}")]
        public ActionResult New()
        {
            DentistaViewModel dentistaViewModel = new DentistaViewModel();
            dentistaViewModel.ListUsuario = ListUsuarios(null);

            return View(dentistaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(DentistaViewModel dentistaViewModel)
        {
            var resultado = "";

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            var usuario = ToViewModel<Usuario, UsuarioViewModel>(_UsuarioService.Query().Where(a => a.NOME.Equals(dentistaViewModel.Usuario_string)).FirstOrDefault());
            dentistaViewModel.Usuario = usuario;

            var dentista = ToModel(dentistaViewModel);

            if (!_DentistaService.Insert(dentista, user_logado, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });

        }

        #endregion

        #region Editar 

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Editar Dentistas\"}")]
        public ActionResult Edit(int id)
        {
            DentistaViewModel dentistaViewModel = new DentistaViewModel();

            dentistaViewModel = ToViewModel(_DentistaService.GetById(id));

            dentistaViewModel.ListUsuario = ListUsuarios(null);
            dentistaViewModel.Usuario_string = dentistaViewModel.Usuario.Nome;

            return View(dentistaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(DentistaViewModel dentistaViewModel)
        {
            var resultado = "";

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            var usuario = ToViewModel<Usuario, UsuarioViewModel>(_UsuarioService.Query().Where(a => a.NOME.Equals(dentistaViewModel.Usuario_string)).FirstOrDefault());
            dentistaViewModel.Usuario = usuario;

            var dentista = ToModel(dentistaViewModel);

            if (!_DentistaService.Update(dentista, user_logado, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });

        }

        #endregion

        #region Excluir 

        [HttpPost]
        public ActionResult Delete(string id)
        {
            int idDentista = int.Parse(id);
            var resultado = "";

            Usuario usuarioAtual = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            if (!ReturnPermission(usuarioAtual, "Excluir Dentistas"))
            {
                return Json(new { Success = false, Response = "Você não tem permissão para esta funcionalidade." });
            }

            if (!_DentistaService.Delete(idDentista, usuarioAtual, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });
        }

        #endregion

        #region Filtrar

        public DentistaViewModel Filtrar(DentistaViewModel dentistaViewModel, FormCollection collection)
        {
            List<DentistaViewModel> ListaFiltrada = new List<DentistaViewModel>();

            ListaFiltrada = dentistaViewModel.ListDentistaViewModel;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Nome"]))
                {
                    var nome = collection["Nome"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Nome.Contains(nome)).ToList();
                    dentistaViewModel.Nome = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["Cro"]))
                {
                    var cro = collection["Cro"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Cro.Contains(cro)).ToList();
                    dentistaViewModel.Cro = cro;
                }

                if (!string.IsNullOrWhiteSpace(collection["Usuario_string"]))
                {
                    var usuario = collection["Usuario_string"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Usuario.Nome.Contains(usuario)).ToList();
                    dentistaViewModel.Usuario_string = usuario;
                }

                if (!string.IsNullOrWhiteSpace(collection["DataDe"]) && !string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Dt_Insert) >= dataDe && Convert.ToDateTime(a.Dt_Insert) < dataAte.AddDays(1)).ToList();

                    dentistaViewModel.DataDe_Filtro = collection["DataDe"];
                    dentistaViewModel.DataAte_Filtro = collection["DataAte"];
                }

                else if (!string.IsNullOrWhiteSpace(collection["DataDe"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Dt_Insert) >= dataDe).ToList();
                    dentistaViewModel.DataDe_Filtro = collection["DataDe"];

                }

                else if (!string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Dt_Insert) < dataAte.AddDays(1)).ToList();
                    dentistaViewModel.DataAte_Filtro = collection["DataAte"];
                }
            }

            dentistaViewModel.ListDentistaViewModel = ListaFiltrada;

            return dentistaViewModel;
        }

        #endregion

        #region Select List

        public SelectList ListUsuarios(object id = null)
        {
            List<string> list = new List<string>();


            var usuarioList = ToListViewModel<Usuario, UsuarioViewModel>(_UsuarioService.QueryAsNoTracking().OrderBy(a => a.NOME).ToList());

            foreach (var item in usuarioList)
            {
                list.Add(item.Nome);
            }

            return new SelectList(list, id);
        }

        #endregion

        #region Validações

        public bool ReturnPermission(Usuario usuario, string func)
        {
            try
            {
                var funcionalidade = _FuncionalidadeService.Query().Where(a => a.NOME.Equals(func)).FirstOrDefault();
                var existeFunc = _UsuarioFuncionalidadeService
                                .Query()
                                .Where(a => a.ID_USUARIO == usuario.ID && a.ID_FUNCIONALIDADE == funcionalidade.ID)
                                .ToList()
                                .Count() > 0 ?
                                true :
                                false;

                return existeFunc;
            }
            catch
            {
                return false;
            }
        }

        #endregion
    }
}