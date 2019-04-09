using SPD.Model.Enums;
using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class PreConsultaController : MapperController<IPreConsultaService, PreConsulta, PreConsultaViewModel>
    {
        private readonly IPreConsultaService _PreConsultaService;
        private readonly IPacienteService _PacienteService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IFuncionalidadeService _FuncionalidadeService;
        private readonly IUsuarioFuncionalidadeService _UsuarioFuncionalidadeService;

        public PreConsultaController(IPreConsultaService preConsultaService,
                                     IPacienteService pacienteService,
                                     IUsuarioService usuarioService,
                                     IFuncionalidadeService funcionalidadeService,
                                     IUsuarioFuncionalidadeService usuarioFuncionalidadeService)
            : base(preConsultaService)
        {
            _PreConsultaService = preConsultaService;
            _PacienteService = pacienteService;
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuarioFuncionalidadeService;
        }

        public ActionResult Teste()
        {

            return View();
        }

        #region List

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Pré Consultas\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            PreConsultaViewModel preConsultaViewModel = new PreConsultaViewModel();

            preConsultaViewModel = ReturnPreConsulta(collection);

            return View(preConsultaViewModel);
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

            PreConsultaViewModel preConsultaViewModel = new PreConsultaViewModel();

            preConsultaViewModel = ReturnPreConsulta(collection);

            int totalRecords = preConsultaViewModel.ListPreConsultaViewModel.Count;

            preConsultaViewModel.ListPreConsultaViewModel = Ordenacao(order, orderDir, preConsultaViewModel.ListPreConsultaViewModel);

            int recFilter = preConsultaViewModel.ListPreConsultaViewModel.Count;

            preConsultaViewModel.ListPreConsultaViewModel = preConsultaViewModel.ListPreConsultaViewModel.Skip(startRec).Take(pageSize).ToList();

            List<object> listToView = new List<object>();

            foreach (var item in preConsultaViewModel.ListPreConsultaViewModel)
            {
                listToView.Add(new
                {
                    item.ID,
                    Paciente = item.Paciente.Nome,
                    Autorizado = item.Autorizado == true ? "Sim" : "Não",
                    item.Convenio,
                    Data = item.Dt_Insert.ToString()

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

        private List<PreConsultaViewModel> Ordenacao(string order, string orderDir, List<PreConsultaViewModel> data)
        {
            // Initialization
            List<PreConsultaViewModel> lst = new List<PreConsultaViewModel>();

            try
            {

                switch (order)
                {
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ID).ToList() : data.OrderBy(p => p.ID).ToList();
                        break;

                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Paciente.Nome).ToList() : data.OrderBy(p => p.Paciente.Nome).ToList();
                        break;

                    case "3":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Autorizado).ToList() : data.OrderBy(p => p.Autorizado).ToList();
                        break;

                    case "4":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Convenio).ToList() : data.OrderBy(p => p.Convenio).ToList();
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

        public PreConsultaViewModel ReturnPreConsulta(FormCollection collection = null)
        {
            PreConsultaViewModel preConsultaViewModel = new PreConsultaViewModel();

            preConsultaViewModel.ListPreConsultaViewModel = ToListViewModel(_PreConsultaService.QueryAsNoTracking().ToList());

            preConsultaViewModel = Filtrar(preConsultaViewModel, collection);

            return preConsultaViewModel;
        }

        #endregion

        #region New 

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Iniciar Pré Consultas\"}")]
        public ActionResult New(FormCollection collection = null)
        {
            PreConsultaViewModel preConsultaViewModel = new PreConsultaViewModel();

            ViewBag.paciente = collection["Paciente_string"] == null ? "" : collection["Paciente_string"].ToString();
            ViewBag.convenio = collection["Conveniado"] == null ? "false" : collection["Conveniado"].ToString().Substring(0, 5).Replace(",", "");
            ViewBag.nomeConvenio = collection["Convenio"] == null ? "" : collection["Convenio"].ToString();
            ViewBag.nrCarterinha = collection["Numero_Carterinha"] == null ? "" : collection["Numero_Carterinha"].ToString();
            ViewBag.NomeResponsavel = collection["Nome_Responsavel"] == null ? "" : collection["Nome_Responsavel"].ToString();
            ViewBag.CpfResponsavel = collection["Cpf_Responsavel"] == null ? "" : collection["Cpf_Responsavel"].ToString();
            ViewBag.Img = collection["img_string_value"] == null ? "" : collection["img_string_value"].ToString();
            ViewBag.Accept = collection["accept_string_value"] == null ? "" : collection["accept_string_value"].ToString();

            preConsultaViewModel.ListNomePaciente = ListNomePacientes(null);

            var mes = DateTime.Now.Month;
            string string_mes = Enum.GetName(typeof(SPD_Enums.Meses), mes);

            ViewBag.Mes = string_mes;

            return View(preConsultaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(PreConsultaViewModel preConsultaViewModel)
        {
            var resultado = "";

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            var paciente = ToViewModel<Paciente, PacienteViewModel>(_PacienteService.Query().Where(a => a.NOME.Equals(preConsultaViewModel.Paciente_string)).FirstOrDefault());
            preConsultaViewModel.Paciente = paciente;

            var preConsulta = ToModel(preConsultaViewModel);

            if (!_PreConsultaService.Insert(preConsulta, user_logado, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });


        }

        [HttpPost]
        public JsonResult AutoCompletePaciente(string prefix)
        {
            //Note : you can bind same list from database  
            var listPaciente = ToListViewModel<Paciente, PacienteViewModel>(_PacienteService.QueryAsNoTracking().ToList());

            listPaciente = listPaciente.Where(a => a.Nome.ToUpper().StartsWith(prefix.ToUpper())).ToList();


            return Json(listPaciente);
        }

        #endregion

        #region Edit 

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Editar Pré Consultas\"}")]
        public ActionResult Edit(int id)
        {
            PreConsultaViewModel preConsultaViewModel = new PreConsultaViewModel();

            preConsultaViewModel = ToViewModel(_PreConsultaService.GetById(id));

            preConsultaViewModel.Paciente_string = preConsultaViewModel.Paciente.Nome;

            return View(preConsultaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(PreConsultaViewModel preConsultaViewModel)
        {
            var resultado = "";

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            var paciente = ToViewModel<Paciente, PacienteViewModel>(_PacienteService.Query().Where(a => a.NOME.Equals(preConsultaViewModel.Paciente_string)).FirstOrDefault());
            preConsultaViewModel.Paciente = paciente;

            var preConsulta = ToModel(preConsultaViewModel);

            if (!_PreConsultaService.Update(preConsulta, user_logado, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });

        }

        #endregion

        #region Delete 

        [HttpPost]
        public ActionResult Delete(string id)
        {
            int idPreConsulta = int.Parse(id);
            var resultado = "";

            Usuario usuarioAtual = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            if (!ReturnPermission(usuarioAtual, "Excluir Pré Consultas"))
            {
                return Json(new { Success = false, Response = "Você não tem permissão para esta funcionalidade." });
            }

            if (!_PreConsultaService.Delete(idPreConsulta, usuarioAtual, out resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });
        }

        #endregion

        #region Filtrar

        public PreConsultaViewModel Filtrar(PreConsultaViewModel preConsultaViewModel, FormCollection collection)
        {
            List<PreConsultaViewModel> ListaFiltrada = new List<PreConsultaViewModel>();

            ListaFiltrada = preConsultaViewModel.ListPreConsultaViewModel;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Paciente_string"]))
                {
                    var nome = collection["Paciente_string"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Paciente.Nome.Contains(nome)).ToList();
                    preConsultaViewModel.Paciente_string = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["Autorizado_string"]))
                {
                    var status = collection["Autorizado_string"].ToString().Replace(",", "") == "true" ? true : false;

                    ListaFiltrada = ListaFiltrada.Where(a => a.Autorizado == status).ToList();
                    preConsultaViewModel.Autorizado_string = status.ToString();
                }

                if (!string.IsNullOrWhiteSpace(collection["Convenio"]))
                {
                    var convenio = collection["Convenio"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Convenio.Contains(convenio)).ToList();
                    preConsultaViewModel.Convenio = convenio;
                }

                if (!string.IsNullOrWhiteSpace(collection["DataDe"]) && !string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Dt_Insert) >= dataDe && Convert.ToDateTime(a.Dt_Insert) < dataAte.AddDays(1)).ToList();

                    preConsultaViewModel.DataDe_Filtro = collection["DataDe"];
                    preConsultaViewModel.DataAte_Filtro = collection["DataAte"];
                }

                else if (!string.IsNullOrWhiteSpace(collection["DataDe"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Dt_Insert) >= dataDe).ToList();
                    preConsultaViewModel.DataDe_Filtro = collection["DataDe"];

                }

                else if (!string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Dt_Insert) < dataAte.AddDays(1)).ToList();
                    preConsultaViewModel.DataAte_Filtro = collection["DataAte"];
                }
            }

            preConsultaViewModel.ListPreConsultaViewModel = ListaFiltrada;

            return preConsultaViewModel;
        }

        #endregion

        #region Select List 

        public SelectList ListNomePacientes(object id = null)
        {
            List<string> list = new List<string>();
            var pacientes = _PacienteService.Query().Select(a => a.NOME).ToList();

            list = pacientes.Distinct().ToList();

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

        #region Outros

        [HttpPost]
        public JsonResult GetPaciente(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
            {
                return Json(new { Success = false, Response = "Selecione um paciente." });
            }
            else
            {
                var paciente = ToViewModel<Paciente, PacienteViewModel>(_PacienteService.Query().Where(a => a.NOME.Equals(nome)).FirstOrDefault());

                var dt_nasc = Convert.ToDateTime(paciente.Data_Nasc);

                int idade = DateTime.Now.Year - dt_nasc.Year;
                if (DateTime.Now.Month < dt_nasc.Month || (DateTime.Now.Month == dt_nasc.Month && DateTime.Now.Day < dt_nasc.Day))
                    idade--;

                var maiorIdade = idade >= 18 ? true : false;

                return Json(new { Success = true, Response = "", Idade = idade, MaiorIdade = maiorIdade });
            }
        }

        #endregion
    }
}