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
    public class AgendaController : MapperController<IAgendaService, Agenda, AgendaViewModel>
    {
        private readonly IAgendaService _AgendaService;
        private readonly IDentistaService _DentistaService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IFuncionalidadeService _FuncionalidadeService;
        private readonly IUsuarioFuncionalidadeService _UsuarioFuncionalidadeService;

        public AgendaController(IAgendaService agendaService,
                                IDentistaService dentistaService,
                                IUsuarioService usuarioService,
                                IFuncionalidadeService funcionalidadeService,
                                IUsuarioFuncionalidadeService usuarioFuncionalidadeService)
            : base(agendaService)
        {
            _AgendaService = agendaService;
            _DentistaService = dentistaService;
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuarioFuncionalidadeService;
        }

        #region List

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Agenda\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            AgendaViewModel agendaViewModel = new AgendaViewModel();

            agendaViewModel = ReturnAgenda(collection);

            return View(agendaViewModel);
        }

        [HttpPost]
        public ActionResult Paginacao(FormCollection collection = null)
        {
            string draw = Request.Form.GetValues("draw")[0];
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            if (pageSize == -1)
            {
                pageSize = 100;
            }

            AgendaViewModel agendaViewModel = new AgendaViewModel();

            agendaViewModel = ReturnAgenda(collection);

            int totalRecords = agendaViewModel.ListAgendaViewModel.Count;

            int recFilter = agendaViewModel.ListAgendaViewModel.Count;

            agendaViewModel.ListAgendaViewModel = agendaViewModel.ListAgendaViewModel.Skip(startRec).Take(pageSize).ToList();

            List<object> listToView = new List<object>();

            foreach (var item in agendaViewModel.ListAgendaViewModel.OrderBy(a => a.Data_Consulta).ToList())
            {
                listToView.Add(new
                {
                    item.ID,
                    //ToDo: agrupar por hora
                    Hora_string = $" Hora: {item.Data_Consulta.Hour}",
                    Hora = item.Data_Consulta.Hour,
                    Dentista = item.Dentista.Nome,
                    Paciente = item.Nome_Paciente,
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

        public AgendaViewModel ReturnAgenda(FormCollection collection = null)
        {
            AgendaViewModel agendaViewModel = new AgendaViewModel
            {
                ListAgendaViewModel = ToListViewModel(_AgendaService.QueryAsNoTracking().ToList())
            };

            agendaViewModel = Filtrar(agendaViewModel, collection);

            return agendaViewModel;
        }

        #endregion

        #region New 

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Adicionar Agenda\"}")]
        public ActionResult New(FormCollection collection = null)
        {
            AgendaViewModel agendaViewModel = new AgendaViewModel();

            return View(agendaViewModel);
        }

        public ActionResult Add(AgendaViewModel agendaViewModel)
        {

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            var agenda = ToModel(agendaViewModel);

            if (!_AgendaService.Insert(agenda, user_logado, out string resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });


        }

        #endregion

        #region Edit 

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Editar Agenda\"}")]
        public ActionResult Edit(int id)
        {
            AgendaViewModel agendaViewModel = new AgendaViewModel();

            agendaViewModel = ToViewModel(_AgendaService.GetById(id));

            agendaViewModel.Dentista_string = agendaViewModel.Dentista.Nome;

            return View(agendaViewModel);
        }

        #endregion

        #region Delete 

        [HttpPost]
        public ActionResult Delete(string id)
        {
            int idAgenda = int.Parse(id);

            Usuario usuarioAtual = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            if (!ReturnPermission(usuarioAtual, "Excluir Agenda"))
            {
                return Json(new { Success = false, Response = "Você não tem permissão para esta funcionalidade." });
            }

            if (!_AgendaService.Delete(idAgenda, usuarioAtual, out string resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });
        }

        #endregion

        #region Filtrar

        public AgendaViewModel Filtrar(AgendaViewModel agendaViewModel, FormCollection collection)
        {
            List<AgendaViewModel> ListaFiltrada = new List<AgendaViewModel>();

            ListaFiltrada = agendaViewModel.ListAgendaViewModel;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Dentista_string"]))
                {
                    var nome = collection["Dentista_string"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Dentista.Nome.Contains(nome)).ToList();
                    agendaViewModel.Dentista_string = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["DataDe"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Dt_Insert).Date == dataDe.Date).ToList();
                    agendaViewModel.DataDe_Filtro = collection["DataDe"];

                }
            }

            agendaViewModel.ListAgendaViewModel = ListaFiltrada;

            return agendaViewModel;
        }

        #endregion

        #region Select List 

        public SelectList ListNomeDentistas(object id = null)
        {
            List<string> list = new List<string>();
            var dentistas = _DentistaService.Query().Select(a => a.NOME).ToList();

            list = dentistas.Distinct().ToList();

            return new SelectList(list, id);
        }

        public SelectList ListHoraDisponivel(object id = null)
        {
            List<string> list = new List<string>();
            // var hora = _AgendaService.Query().Select(a => a.NOME).ToList();

            // list = hora.Distinct().ToList();

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