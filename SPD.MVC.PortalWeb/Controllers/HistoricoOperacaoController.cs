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
    public class HistoricoOperacaoController : MapperController<IHistoricoOperacaoService, HistoricoOperacao, HistoricoOperacaoViewModel>
    {
        private readonly IHistoricoOperacaoService _HistoricoOperacaoService;
        private readonly ITipoOperacaoService _TipoOperacaoService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IFuncionalidadeService _FuncionalidadeService;

        public HistoricoOperacaoController(IHistoricoOperacaoService historicoOperacaoService,
                                           ITipoOperacaoService tipoOperacaoService,
                                           IUsuarioService usuarioService,
                                           IFuncionalidadeService funcionalidadeService)
            : base(historicoOperacaoService)
        {
            _HistoricoOperacaoService = historicoOperacaoService;
            _TipoOperacaoService = tipoOperacaoService;
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
        }

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Visualizar Histórico de Operação\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            HistoricoOperacaoViewModel historicoOperacaoViewModel = new HistoricoOperacaoViewModel();

            historicoOperacaoViewModel = ReturnHistoricoOperacao(collection);

            // historicoOperacaoViewModel.DataDe_Filtro = (Convert.ToDateTime(historicoOperacaoViewModel.DataDe_Filtro)).ToShortDateString();

            return View(historicoOperacaoViewModel);
        }

        [HttpPost]
        public PartialViewResult _List(FormCollection collection = null)
        {
            return PartialView();
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

            HistoricoOperacaoViewModel historicoOperacaoViewModel = new HistoricoOperacaoViewModel();

            historicoOperacaoViewModel = ReturnHistoricoOperacao(collection);

            int totalRecords = historicoOperacaoViewModel.ListHistoricoOperacaoViewModels.Count;

            historicoOperacaoViewModel.ListHistoricoOperacaoViewModels = Ordenacao(order, orderDir, historicoOperacaoViewModel.ListHistoricoOperacaoViewModels);

            int recFilter = historicoOperacaoViewModel.ListHistoricoOperacaoViewModels.Count;

            historicoOperacaoViewModel.ListHistoricoOperacaoViewModels = historicoOperacaoViewModel.ListHistoricoOperacaoViewModels.Skip(startRec).Take(pageSize).ToList();

            List<object> listToView = new List<object>();

            foreach (var item in historicoOperacaoViewModel.ListHistoricoOperacaoViewModels)
            {
                listToView.Add(new
                {
                    item.ID,
                    item.Descricao,
                    Nome_Tipo_Operacao = item.TipoOperacao.Descricao_Tipo_Operacao,
                    Nome_Usuario = item.Usuario.Nome,
                    Nome_Funcionalidade = item.Funcionalidade.Nome,
                    DataOperacao = item.Dt_Operacao.ToString()
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

        private List<HistoricoOperacaoViewModel> Ordenacao(string order, string orderDir, List<HistoricoOperacaoViewModel> data)
        {
            // Initialization
            List<HistoricoOperacaoViewModel> lst = new List<HistoricoOperacaoViewModel>();

            try
            {
                // Sorting   
                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Descricao).ToList() : data.OrderBy(p => p.Descricao).ToList();
                        break;

                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TipoOperacao.Descricao_Tipo_Operacao).ToList() : data.OrderBy(p => p.TipoOperacao.Descricao_Tipo_Operacao).ToList();
                        break;

                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Usuario.Nome).ToList() : data.OrderBy(p => p.Usuario.Nome).ToList();
                        break;

                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Funcionalidade.Nome).ToList() : data.OrderBy(p => p.Funcionalidade.Nome).ToList();
                        break;

                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Dt_Operacao).ToList() : data.OrderBy(p => p.Dt_Operacao).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Dt_Operacao).ToList() : data.OrderBy(p => p.Dt_Operacao).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return lst;
        }

        public HistoricoOperacaoViewModel ReturnHistoricoOperacao(FormCollection collection = null)
        {
            HistoricoOperacaoViewModel historicoOperacaoViewModel = new HistoricoOperacaoViewModel();

            historicoOperacaoViewModel.ListHistoricoOperacaoViewModels = ToListViewModel(_HistoricoOperacaoService.QueryAsNoTracking().ToList());

            historicoOperacaoViewModel.ListTipoOperacao = ListTipoOperacao(historicoOperacaoViewModel, null);
            historicoOperacaoViewModel.ListUsuario = ListUsuario(historicoOperacaoViewModel, null);
            historicoOperacaoViewModel.ListFuncionalidade = ListFuncionalidade(historicoOperacaoViewModel, null);

            historicoOperacaoViewModel = Filtrar(historicoOperacaoViewModel, collection);

            return historicoOperacaoViewModel;
        }

        #region Filtrar

        public HistoricoOperacaoViewModel Filtrar(HistoricoOperacaoViewModel historicoOperacaoViewModel, FormCollection collection)
        {
            List<HistoricoOperacaoViewModel> ListaFiltrada = new List<HistoricoOperacaoViewModel>();

            ListaFiltrada = historicoOperacaoViewModel.ListHistoricoOperacaoViewModels;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Descricao_Filtro"]))
                {
                    var descricao = collection["Descricao_Filtro"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Descricao.Contains(descricao)).ToList();
                    historicoOperacaoViewModel.Descricao_Filtro = descricao;
                }

                if (!string.IsNullOrWhiteSpace(collection["TipoOperacao_Filtro"]))
                {
                    var tp_oper = collection["TipoOperacao_Filtro"].ToString();

                    var id = _TipoOperacaoService.Query().Where(a => a.DESCRICAO_TIPO_OPERACAO.Equals(tp_oper)).FirstOrDefault().ID;

                    ListaFiltrada = ListaFiltrada.Where(a => a.ID_Tipo_Operacao == id).ToList();
                    historicoOperacaoViewModel.TipoOperacao_Filtro = tp_oper;
                }

                if (!string.IsNullOrWhiteSpace(collection["Usuario_Filtro"]))
                {
                    var usuario = collection["Usuario_Filtro"].ToString();

                    var id = _UsuarioService.Query().Where(a => a.NOME.Equals(usuario)).FirstOrDefault().ID;

                    ListaFiltrada = ListaFiltrada.Where(a => a.ID_Usuario == id).ToList();
                    historicoOperacaoViewModel.Usuario_Filtro = usuario;

                }

                if (!string.IsNullOrWhiteSpace(collection["Funcionalidade_Filtro"]))
                {
                    var funcionalidade = collection["Funcionalidade_Filtro"].ToString();

                    var id = _FuncionalidadeService.Query().Where(a => a.NOME.Equals(funcionalidade)).FirstOrDefault().ID;

                    ListaFiltrada = ListaFiltrada.Where(a => a.ID_Funcionalidade == id).ToList();
                    historicoOperacaoViewModel.Funcionalidade_Filtro = funcionalidade;

                }

                if (!string.IsNullOrWhiteSpace(collection["DataDe"]) && !string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Dt_Operacao >= dataDe && a.Dt_Operacao < dataAte.AddDays(1)).ToList();

                    historicoOperacaoViewModel.DataDe_Filtro = collection["DataDe"];
                    historicoOperacaoViewModel.DataAte_Filtro = collection["DataAte"];
                }

                else if (!string.IsNullOrWhiteSpace(collection["DataDe"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Dt_Operacao >= dataDe).ToList();
                    historicoOperacaoViewModel.DataDe_Filtro = collection["DataDe"];

                }

                else if (!string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Dt_Operacao < dataAte.AddDays(1)).ToList();
                    historicoOperacaoViewModel.DataAte_Filtro = collection["DataAte"];
                }

            }

            historicoOperacaoViewModel.ListHistoricoOperacaoViewModels = ListaFiltrada;

            return historicoOperacaoViewModel;
        }

        #endregion

        #region Get List

        public SelectList ListTipoOperacao(HistoricoOperacaoViewModel historicoOperacaoViewModel, object id = null)
        {
            var listID = historicoOperacaoViewModel.ListHistoricoOperacaoViewModels.Select(a => a.ID_Tipo_Operacao).Distinct().ToList();
            var tipo_oper = _TipoOperacaoService.Query().Where(a => listID.Contains(a.ID)).ToList();

            var list = tipo_oper.OrderBy(a => a.DESCRICAO_TIPO_OPERACAO).Select(a => a.DESCRICAO_TIPO_OPERACAO).ToList();

            return new SelectList(list, id);
        }

        public SelectList ListUsuario(HistoricoOperacaoViewModel historicoOperacaoViewModel, object id = null)
        {
            var listID = historicoOperacaoViewModel.ListHistoricoOperacaoViewModels.Select(a => a.ID_Usuario).Distinct().ToList();
            var usuarios = _UsuarioService.Query().Where(a => listID.Contains(a.ID)).ToList();

            var list = usuarios.OrderBy(a => a.NOME).Select(a => a.NOME).ToList();

            return new SelectList(list, id);
        }

        public SelectList ListFuncionalidade(HistoricoOperacaoViewModel historicoOperacaoViewModel, object id = null)
        {
            var listID = historicoOperacaoViewModel.ListHistoricoOperacaoViewModels.Select(a => a.ID_Funcionalidade).Distinct().ToList();
            var funcionalidades = _FuncionalidadeService.Query().Where(a => listID.Contains(a.ID)).ToList();

            var list = funcionalidades.OrderBy(a => a.NOME).Select(a => a.NOME).ToList();

            return new SelectList(list, id);
        }

        #endregion
    }
}