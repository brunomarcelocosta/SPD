using SPD.Model.Model;
using SPD.Model.Util;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Global;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly ISessaoUsuarioService _SessaoUsuarioService;

        public UsuarioController(IUsuarioService usuarioService,
                                 IFuncionalidadeService funcionalidadeService,
                                 IUsuarioFuncionalidadeService usuarioFuncionalidadeService,
                                 ISessaoUsuarioService sessaoUsuarioService)
            : base(usuarioService)
        {
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuarioFuncionalidadeService;
            _SessaoUsuarioService = sessaoUsuarioService;
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

        #region Novo

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Novos Usuários\"}")]
        public ActionResult New()
        {
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();
            var funcionalidades = ToListViewModel<Funcionalidade, FuncionalidadeViewModel>(_FuncionalidadeService.Query().ToList());

            foreach (var item in funcionalidades)
            {
                item.Selecionado = false;
            }

            usuarioViewModel.ListFuncionalidadeViewModel = funcionalidades;

            return View(usuarioViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(UsuarioViewModel usuarioViewModel)
        {
            var resultado = "";
            var newusuario = ToModel(usuarioViewModel);

            List<FuncionalidadeViewModel> funcionalidades = new List<FuncionalidadeViewModel>();
            funcionalidades = usuarioViewModel.ListFuncionalidadeViewModel;

            var user_logado = this.ApplicationService.GetById(this.GetAuthenticationFromSession().ID);

            var ListUsuarioFunc = ToListViewModel<UsuarioFuncionalidade, UsuarioFuncionalidadeViewModel>(_UsuarioFuncionalidadeService.Query().ToList());

            List<UsuarioFuncionalidadeViewModel> idFunc_ADD = new List<UsuarioFuncionalidadeViewModel>();

            foreach (var item in funcionalidades)
            {
                if (item.Selecionado == true)
                {
                    idFunc_ADD.Add(new UsuarioFuncionalidadeViewModel
                    {
                        Usuario = usuarioViewModel,
                        Funcionalidade = item
                    });
                }
            }

            var usuario = ToModel(usuarioViewModel);
            var usuarioFuncionalidades_ADD = ToListModel<UsuarioFuncionalidade, UsuarioFuncionalidadeViewModel>(idFunc_ADD);

            var emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
            var pwdFrom = ConfigurationManager.AppSettings["pwdFrom"].ToString();

            if (!_UsuarioService.AddNewUser(usuario, user_logado, usuarioFuncionalidades_ADD, emailFrom, pwdFrom, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }
            else
            {
                return Json(new { Success = true, Nome = usuario.NOME });
            }
        }

        #endregion

        #region Editar

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Editar Usuários\"}")]
        public ActionResult Edit(int id)
        {
            UsuarioViewModel usuarioViewModel = new UsuarioViewModel();

            usuarioViewModel = ToViewModel(_UsuarioService.GetById(id));

            if (usuarioViewModel == null)
            {
                return Redirect(GlobalConstants.Modules.Action(GlobalConstants.Modules.Web.Url, "Home", "Index"));
            }

            var usuariosFuncionalidades = ToListViewModel<UsuarioFuncionalidade, UsuarioFuncionalidadeViewModel>(_UsuarioFuncionalidadeService.Query().Where(a => a.ID_USUARIO == usuarioViewModel.ID).ToList());
            var funcionalidades = ToListViewModel<Funcionalidade, FuncionalidadeViewModel>(_FuncionalidadeService.Query().ToList());

            foreach (var item in funcionalidades)
            {
                var existe = usuariosFuncionalidades.Where(a => a.ID_FUNCIONALIDADE == item.ID).ToList().Count() > 0 ? true : false;
                item.Selecionado = existe;
            }

            usuarioViewModel.ListFuncionalidadeViewModel = funcionalidades;

            return View(usuarioViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(UsuarioViewModel usuarioViewModel)
        {
            var resultado = "";
            bool funcAlterado = false;

            List<FuncionalidadeViewModel> funcionalidades = new List<FuncionalidadeViewModel>();
            funcionalidades = usuarioViewModel.ListFuncionalidadeViewModel;

            var user_logado = this.ApplicationService.GetById(this.GetAuthenticationFromSession().ID);

            var ListUsuarioFunc = ToListViewModel<UsuarioFuncionalidade, UsuarioFuncionalidadeViewModel>(_UsuarioFuncionalidadeService.Query().ToList());

            List<UsuarioFuncionalidadeViewModel> idFunc_ADD = new List<UsuarioFuncionalidadeViewModel>();
            List<UsuarioFuncionalidadeViewModel> idFunc_DEL = new List<UsuarioFuncionalidadeViewModel>();

            foreach (var item in funcionalidades)
            {
                if (item.Selecionado == true)
                {
                    var existe = ListUsuarioFunc.Where(a => a.ID_FUNCIONALIDADE == item.ID && a.ID_Usuario == usuarioViewModel.ID).ToList().Count() > 0 ? true : false;

                    if (!existe)
                    {
                        idFunc_ADD.Add(new UsuarioFuncionalidadeViewModel
                        {
                            Usuario = usuarioViewModel,
                            Funcionalidade = item
                        });
                    }
                }

                else
                {
                    var existe = ListUsuarioFunc.Where(a => a.ID_FUNCIONALIDADE == item.ID && a.ID_Usuario == usuarioViewModel.ID).ToList().Count() > 0 ? true : false;

                    if (existe)
                    {
                        idFunc_DEL.Add(new UsuarioFuncionalidadeViewModel
                        {
                            Usuario = usuarioViewModel,
                            Funcionalidade = item
                        });
                    }
                }
            }

            var usuario = ToModel(usuarioViewModel);
            var usuarioFuncionalides_ADD = ToListModel<UsuarioFuncionalidade, UsuarioFuncionalidadeViewModel>(idFunc_ADD);
            var usuarioFuncionalidades_DEL = ToListModel<UsuarioFuncionalidade, UsuarioFuncionalidadeViewModel>(idFunc_DEL); ;

            if (_UsuarioService.UpdateUser(usuario, user_logado, usuarioFuncionalides_ADD, usuarioFuncionalidades_DEL, out resultado))
            {
                if ((usuarioFuncionalides_ADD.Count > 0) || (usuarioFuncionalidades_DEL.Count > 0))
                {
                    funcAlterado = true;

                    SessaoUsuario sessaoUsuarioDesconectar = this._SessaoUsuarioService.GetSessaoByUsuarioID(usuario.ID);
                    if (sessaoUsuarioDesconectar != null)
                    {
                        Job.ScheduleNewJob(180, () =>
                        {
                            this._UsuarioService.Desconectar(this._UsuarioService.GetById(usuario.ID));

                        }).WithNotification("Você será desconectado em {0} por alteração na funcionalidade de acesso.Por favor, salve seus trabalhos correntes.", true).OfDangerType().NotifyUser(usuario.ID);

                    }

                    return Json(new { Success = true, Nome = usuario.NOME, AlteracaoFunc = funcAlterado, IdUsuario = usuario.ID });
                }

                return Json(new { Success = true, Nome = usuario.NOME, AlteracaoFunc = funcAlterado, IdUsuario = usuario.ID });

            }
            else
            {
                return Json(new { Success = false, Response = resultado });
            }
        }

        #endregion

        #region Excluir

        [HttpPost]
        public ActionResult Delete(string id)
        {
            int idUser = int.Parse(id);
            var resultado = "";

            Usuario usuarioAtual = this.ApplicationService.GetById(this.GetAuthenticationFromSession().ID);

            if (!ReturnPermission(usuarioAtual, "Excluir Usuários"))
            {
                return Json(new { Success = false, Response = "Você não tem permissão para esta funcionalidade." });
            }

            if (!_UsuarioService.DeleteUser(idUser, usuarioAtual, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });
        }

        #endregion

        #region Redefinir Senha

        [HttpPost]
        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Redefinir Senha\"}")]
        public ActionResult RedefinirSenha(string sLogin)
        {
            try
            {
                var emailFrom = ConfigurationManager.AppSettings["emailFrom"].ToString();
                var pwdFrom = ConfigurationManager.AppSettings["pwdFrom"].ToString();

                if (this.ApplicationService.RedefinirSenha(sLogin, emailFrom, pwdFrom, null) > 0)
                {

                    return Json(new { Success = true, Response = "Senha redefinida com sucesso." });
                }
                else
                {
                    return Json(new { Success = false, Response = "Não foi possível redefinir a senha." });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Response = "Não foi possível redefinir a senha." });
            }
        }

        #endregion

        #region Desbloquear Usuário

        [HttpPost]
        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Desbloquear Usuários\"}")]
        public ActionResult DesbloquearUsuario(UsuarioViewModel usuarioViewModel)
        {
            try
            {
                Usuario usuario = ToModel(usuarioViewModel);

                if (this.ApplicationService.Desbloquear(usuario, this.GetAuthenticationFromSession().ID))
                {
                    return Json(new { Success = true, Response = "Usuário Desbloqueado." });
                }
                else
                {
                    return Json(new { Success = false, Response = "Não foi possível desbloquear o usuário." });
                }

            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Response = "Não foi possível desbloquear o usuário." });
            }

        }

        #endregion

        #region Desconectar Usuário

        [HttpGet]
        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Desconectar Usuários\"}")]
        public ActionResult Desconecta(int usuarioID)
        {
            this._UsuarioService.Desconectar(this._UsuarioService.GetById(usuarioID));

            return Json(new { Success = true });
        }

        #endregion

        #region Filtrar

        public UsuarioViewModel Filtrar(UsuarioViewModel usuarioViewModel, FormCollection collection)
        {
            List<UsuarioViewModel> ListaFiltrada = new List<UsuarioViewModel>();

            ListaFiltrada = usuarioViewModel.ListUsuarioViewModel;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["NomeFiltro"]))
                {
                    var nome = collection["NomeFiltro"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Nome.Contains(nome)).ToList();
                    usuarioViewModel.Nome = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["EmailFiltro"]))
                {
                    var email = collection["EmailFiltro"].ToString();

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