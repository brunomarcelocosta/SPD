using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class PacienteController : MapperController<IPacienteService, Paciente, PacienteViewModel>
    {
        private readonly IAgendaService _AgendaService;
        private readonly IPacienteService _PacienteService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IFuncionalidadeService _FuncionalidadeService;
        private readonly IUsuarioFuncionalidadeService _UsuarioFuncionalidadeService;

        public PacienteController(IAgendaService agendaService,
                                  IPacienteService pacienteService,
                                  IUsuarioService usuarioService,
                                  IFuncionalidadeService funcionalidadeService,
                                  IUsuarioFuncionalidadeService usuarioFuncionalidadeService)
                   : base(pacienteService)
        {
            _AgendaService = agendaService;
            _PacienteService = pacienteService;
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuarioFuncionalidadeService;
        }

        #region List 

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Pacientes\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            PacienteViewModel pacienteViewModel = new PacienteViewModel();

            pacienteViewModel = ReturnPaciente(collection);

            return View(pacienteViewModel);
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

            PacienteViewModel pacienteViewModel = new PacienteViewModel();

            pacienteViewModel = ReturnPaciente(collection);

            int totalRecords = pacienteViewModel.ListPacienteViewModel.Count;

            pacienteViewModel.ListPacienteViewModel = Ordenacao(order, orderDir, pacienteViewModel.ListPacienteViewModel);

            int recFilter = pacienteViewModel.ListPacienteViewModel.Count;

            pacienteViewModel.ListPacienteViewModel = pacienteViewModel.ListPacienteViewModel.Skip(startRec).Take(pageSize).ToList();

            List<object> listToView = new List<object>();

            foreach (var item in pacienteViewModel.ListPacienteViewModel)
            {
                listToView.Add(new
                {
                    item.ID,
                    item.Nome,
                    item.Email,
                    Data_Nasc = item.Data_Nasc,
                    Cpf = Convert.ToUInt64(item.Cpf).ToString(@"000\.000\.000\-00"),
                    item.Celular,
                    Ativo = item.Ativo == true ? "Ativo" : "Inativo"
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

        private List<PacienteViewModel> Ordenacao(string order, string orderDir, List<PacienteViewModel> data)
        {
            // Initialization
            List<PacienteViewModel> lst = new List<PacienteViewModel>();

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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email).ToList() : data.OrderBy(p => p.Email).ToList();
                        break;

                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Data_Nasc).ToList() : data.OrderBy(p => p.Data_Nasc).ToList();
                        break;

                    case "5":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Cpf).ToList() : data.OrderBy(p => p.Cpf).ToList();
                        break;

                    case "6":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Celular).ToList() : data.OrderBy(p => p.Celular).ToList();
                        break;

                    case "7":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Ativo).ToList() : data.OrderBy(p => p.Ativo).ToList();
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

        public PacienteViewModel ReturnPaciente(FormCollection collection = null)
        {
            PacienteViewModel pacienteViewModel = new PacienteViewModel();

            pacienteViewModel.ListPacienteViewModel = ToListViewModel(_PacienteService.QueryAsNoTracking().ToList());

            pacienteViewModel = Filtrar(pacienteViewModel, collection);

            return pacienteViewModel;
        }

        #endregion

        #region Novo

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Adicionar Pacientes\"}")]
        public ActionResult New()
        {
            PacienteViewModel pacienteViewModel = new PacienteViewModel();
            pacienteViewModel.ListEstadoCivil = ListEstadoCivil(null);
            pacienteViewModel.ListAgendaDia = BuscaAgenda(null);

            return View(pacienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PacienteViewModel pacienteViewModel)
        {
            if (pacienteViewModel.srcImage != null)
            {
                pacienteViewModel.Foto = Convert.FromBase64String(pacienteViewModel.srcImage.Substring("data:image/jpeg;base64,".Length));
            }
            pacienteViewModel.Rg = pacienteViewModel.Rg.Replace(".", "").Replace("-", "");
            pacienteViewModel.Cpf = pacienteViewModel.Cpf.Replace(".", "").Replace("-", "");
            pacienteViewModel.Ativo = true;

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            var paciente = ToModel(pacienteViewModel);

            //var id_agenda = _AgendaService.GetById(int.Parse(pacienteViewModel.Agenda)).ID;

            if (!_PacienteService.Insert(paciente, user_logado, out string resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });

        }

        #endregion

        #region Editar

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Editar Pacientes\"}")]
        public ActionResult Edit(int id)
        {
            PacienteViewModel pacienteViewModel = new PacienteViewModel();

            pacienteViewModel = ToViewModel(_PacienteService.GetById(id));
            pacienteViewModel.ExisteImg = false;
            if (pacienteViewModel.Foto.Count() > 0)
            {
                pacienteViewModel.srcImage = $"data:image/jpeg;base64,{Convert.ToBase64String(pacienteViewModel.Foto)}";
                pacienteViewModel.ExisteImg = true;

            }
            pacienteViewModel.ListEstadoCivil = ListEstadoCivil(null);

            return View(pacienteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(PacienteViewModel pacienteViewModel)
        {
            var resultado = "";
            if (pacienteViewModel.srcImage != null)
            {
                pacienteViewModel.Foto = Convert.FromBase64String(pacienteViewModel.srcImage.Substring("data:image/jpeg;base64,".Length));
            }
            pacienteViewModel.Rg = pacienteViewModel.Rg.Replace(".", "").Replace("-", "");
            pacienteViewModel.Cpf = pacienteViewModel.Cpf.Replace(".", "").Replace("-", "");
            pacienteViewModel.Ativo = true;
            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            var paciente = ToModel(pacienteViewModel);

            if (!_PacienteService.Update(paciente, user_logado, out resultado))
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
            int idPaciente = int.Parse(id);
            var resultado = "";

            Usuario usuarioAtual = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            if (!ReturnPermission(usuarioAtual, "Excluir Pacientes"))
            {
                return Json(new { Success = false, Response = "Você não tem permissão para esta funcionalidade." });
            }

            if (!_PacienteService.Delete(idPaciente, usuarioAtual, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });
        }

        #endregion

        #region Filtrar

        public PacienteViewModel Filtrar(PacienteViewModel pacienteViewModel, FormCollection collection)
        {
            List<PacienteViewModel> ListaFiltrada = new List<PacienteViewModel>();

            ListaFiltrada = pacienteViewModel.ListPacienteViewModel;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Nome"]))
                {
                    var nome = collection["Nome"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Nome.Contains(nome)).ToList();
                    pacienteViewModel.Nome = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["Cpf"]))
                {
                    var cpf = collection["Cpf"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Cpf.Contains(cpf)).ToList();
                    pacienteViewModel.Cpf = cpf;
                }

                if (!string.IsNullOrWhiteSpace(collection["Email"]))
                {
                    var email = collection["Email"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Email.Contains(email)).ToList();
                    pacienteViewModel.Email = email;
                }

                if (!string.IsNullOrWhiteSpace(collection["isAtivoFiltro"]))
                {
                    var status = collection["isAtivoFiltro"].ToString() == "true" ? true : false;

                    ListaFiltrada = ListaFiltrada.Where(a => a.Ativo == status).ToList();
                    pacienteViewModel.isAtivoFiltro = status.ToString();
                }

                if (!string.IsNullOrWhiteSpace(collection["DataDe"]) && !string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Data_Nasc) >= dataDe && Convert.ToDateTime(a.Data_Nasc) < dataAte.AddDays(1)).ToList();

                    pacienteViewModel.DataDe_Filtro = collection["DataDe"];
                    pacienteViewModel.DataAte_Filtro = collection["DataAte"];
                }

                else if (!string.IsNullOrWhiteSpace(collection["DataDe"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Data_Nasc) >= dataDe).ToList();
                    pacienteViewModel.DataDe_Filtro = collection["DataDe"];

                }

                else if (!string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Data_Nasc) < dataAte.AddDays(1)).ToList();
                    pacienteViewModel.DataAte_Filtro = collection["DataAte"];
                }
            }

            pacienteViewModel.ListPacienteViewModel = ListaFiltrada;

            return pacienteViewModel;
        }

        #endregion

        #region Select List

        public SelectList ListEstadoCivil(object id = null)
        {
            List<string> list = new List<string>();

            foreach (var enums in Enum.GetNames(typeof(EstadosCivis)))
            {
                list.Add(enums);
            }

            return new SelectList(list, id);
        }

        public SelectList BuscaAgenda(object id = null)
        {
            List<string> list = new List<string>();
            var list_dt = new List<DateTime>();

            var list_dt_string = _AgendaService
                                 .QueryAsNoTracking()
                                 .Select(a => a.DATA_CONSULTA)
                                 .Distinct()
                                 .ToList();

            list_dt_string.ToList().ForEach(a => list_dt.Add(Convert.ToDateTime(a).Date));

            list_dt = list_dt.Where(a => a >= DateTime.Now.Date).ToList();

            list = list_dt.Select(a => a.ToShortDateString()).ToList();

            return new SelectList(list, id);
        }

        public JsonResult BuscaHorarioPaciente(string dia)
        {
            var horarios = _AgendaService
                           .QueryAsNoTracking()
                           .Where(a => a.DATA_CONSULTA.Equals(dia))
                           .OrderBy(a => a.HORA_INICIO)
                           .Select(a => a.HORA_INICIO)
                           .ToList();

            return Json(horarios, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscaPacienteAgenda(string hora, string dia)
        {
            var paciente = _AgendaService
                           .QueryAsNoTracking()
                           .Where(a => a.HORA_INICIO.Equals(hora) && a.DATA_CONSULTA.Equals(dia))
                           .FirstOrDefault();

            return Json(paciente.NOME_PACIENTE, JsonRequestBehavior.AllowGet);
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

        #region WebService CEP

        public ActionResult WS_GetCEP(string cep)
        {
            var jsonCEP = WsCEP.GetCEP(cep.Replace("-", ""));

            // obter ID da UF do cep
            if (jsonCEP.erro && string.IsNullOrEmpty(jsonCEP.uf))
            {

                if (jsonCEP.erroMsg.ToString().Contains("(404)"))
                {
                    jsonCEP.erroMsg = "CEP não encontrado.";
                }
            }

            return Json(jsonCEP);
        }

        #endregion
    }
}