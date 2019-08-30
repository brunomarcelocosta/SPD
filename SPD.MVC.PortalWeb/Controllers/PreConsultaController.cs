using SPD.Model.Enums;
using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IAssinaturaService _AssinaturaService;
        private readonly IAgendaService _AgendaService;

        public PreConsultaController(IPreConsultaService preConsultaService,
                                     IPacienteService pacienteService,
                                     IUsuarioService usuarioService,
                                     IFuncionalidadeService funcionalidadeService,
                                     IUsuarioFuncionalidadeService usuarioFuncionalidadeService,
                                     IAssinaturaService assinaturaService,
                                     IAgendaService agendaService)
            : base(preConsultaService)
        {
            _PreConsultaService = preConsultaService;
            _PacienteService = pacienteService;
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuarioFuncionalidadeService;
            _AssinaturaService = assinaturaService;
            _AgendaService = agendaService;
        }

        #region List

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Pré Atendimentos\"}")]
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
            int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
            int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);
            if (pageSize == -1)
            {
                pageSize = 50;
            }

            PreConsultaViewModel preConsultaViewModel = new PreConsultaViewModel();

            preConsultaViewModel = ReturnPreConsulta(collection);

            int totalRecords = preConsultaViewModel.ListPreConsultaViewModel.Count;

            int recFilter = preConsultaViewModel.ListPreConsultaViewModel.Count;

            preConsultaViewModel.ListPreConsultaViewModel = preConsultaViewModel.ListPreConsultaViewModel.Skip(startRec).Take(pageSize).ToList();

            List<object> listToView = new List<object>();

            foreach (var item in preConsultaViewModel.ListPreConsultaViewModel.OrderBy(a => a.Agenda.Dentista.Nome).ThenBy(a => a.Agenda.Hora_Inicio))
            {
                listToView.Add(new
                {
                    item.ID,
                    Dentista = item.Agenda.Dentista.Nome,
                    Horario = item.Agenda.Hora_Inicio,
                    Paciente = item.Agenda.Nome_Paciente,
                    Autorizado = item.Autorizado == true ? "Sim" : "Não",
                    item.Convenio,
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

        public PreConsultaViewModel ReturnPreConsulta(FormCollection collection = null)
        {
            PreConsultaViewModel preConsultaViewModel = new PreConsultaViewModel
            {
                ListPreConsultaViewModel = ToListViewModel(_PreConsultaService.QueryAsNoTracking().ToList())
            };

            preConsultaViewModel = Filtrar(preConsultaViewModel, collection);

            return preConsultaViewModel;
        }

        #endregion

        #region New 

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Iniciar Pré Atendimentos\"}")]
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

        public ActionResult Add(PreConsultaViewModel preConsultaViewModel)
        {
            var datenow = DateTime.Now.ToShortDateString();

            var id_paciente = _PacienteService
                          .Query()
                          .Where(a => a.NOME.Equals(preConsultaViewModel.Paciente_string))
                          .FirstOrDefault().ID;

            var agenda = ToViewModel<Agenda, AgendaViewModel>
            (
                         _AgendaService
                         .QueryAsNoTracking()
                         .Where(a => a.ID_PACIENTE == id_paciente && a.DATA_CONSULTA.Equals(datenow))
                         .FirstOrDefault()
            );

            var assinatura = new AssinaturaViewModel();

            if (!preConsultaViewModel.Maior_Idade)
            {
                assinatura.NOME_RESPONSAVEL = preConsultaViewModel.Nome_Responsavel;
                assinatura.CPF_RESPONSAVEL = preConsultaViewModel.Cpf_Responsavel;

                var img = preConsultaViewModel.Img_string.Substring("data:image/png;base64,".Length);

                assinatura.ASSINATURA = Convert.FromBase64String(img);
                assinatura.DT_INSERT = DateTime.Now;
            }

            var viewModelToBD = new PreConsultaViewModel
            {
                Agenda = agenda,
                Assinatura = assinatura.ASSINATURA == null ? null : assinatura,
                Maior_Idade = preConsultaViewModel.Maior_Idade,
                Autorizado = true,
                Consulta_Iniciada = false,
                Convenio = string.IsNullOrWhiteSpace(preConsultaViewModel.Convenio) ? "Particular" : preConsultaViewModel.Convenio,
                Numero_Carterinha = string.IsNullOrWhiteSpace(preConsultaViewModel.Numero_Carterinha) ? "" : preConsultaViewModel.Numero_Carterinha,
                Dt_Insert = DateTime.Now
            };

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            var preConsulta = ToModel(viewModelToBD);

            if (!_PreConsultaService.Insert(preConsulta, user_logado, out string resultado))
            {
                return Json(new { Success = false, Response = resultado });
            }

            return Json(new { Success = true });


        }

        #endregion

        #region Edit 

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Editar Pré Atendimentos\"}")]
        public ActionResult Edit(int id)
        {
            PreConsultaViewModel preConsultaViewModel = new PreConsultaViewModel();

            preConsultaViewModel = ToViewModel(_PreConsultaService.GetById(id));

            preConsultaViewModel.Paciente_string = preConsultaViewModel.Agenda.Nome_Paciente;
            var paciete = _PacienteService.GetById(preConsultaViewModel.Agenda.ID_Paciente.Value);
            preConsultaViewModel.Conveniado = string.IsNullOrWhiteSpace(preConsultaViewModel.Convenio) ? false : preConsultaViewModel.Convenio.Equals("Particular") ? false : true;
            preConsultaViewModel.particular = preConsultaViewModel.Conveniado ? false : true;

            var dt_nasc = Convert.ToDateTime(paciete.DATA_NASC);

            int idade = DateTime.Now.Year - dt_nasc.Year;
            if (DateTime.Now.Month < dt_nasc.Month || (DateTime.Now.Month == dt_nasc.Month && DateTime.Now.Day < dt_nasc.Day))
                idade--;

            preConsultaViewModel.Idade = idade + " anos";

            if (preConsultaViewModel.ID_Assinatura != 0 && preConsultaViewModel.ID_Assinatura != null)
            {
                var assinatura = ToViewModel<Assinatura, AssinaturaViewModel>(_AssinaturaService.GetById(preConsultaViewModel.ID_Assinatura.Value));

                preConsultaViewModel.Nome_Responsavel = assinatura.NOME_RESPONSAVEL;
                preConsultaViewModel.Cpf_Responsavel = assinatura.CPF_RESPONSAVEL;
                preConsultaViewModel.Img_string = $"data:image/png;base64,{Convert.ToBase64String(assinatura.ASSINATURA)}";
            }

            var mes = DateTime.Now.Month;
            string string_mes = Enum.GetName(typeof(SPD_Enums.Meses), mes);

            ViewBag.Mes = string_mes;

            return View(preConsultaViewModel);
        }

        public ActionResult Update(PreConsultaViewModel preConsultaViewModel)
        {

            var preConsulta = ToViewModel(_PreConsultaService.GetById(preConsultaViewModel.ID));

            preConsulta.ID_Agenda = preConsultaViewModel.ID_Agenda;
            preConsulta.Autorizado = true;
            preConsulta.Consulta_Iniciada = false;
            preConsulta.Convenio = string.IsNullOrWhiteSpace(preConsultaViewModel.Convenio) ? "Particular" : preConsultaViewModel.Convenio;
            preConsulta.Numero_Carterinha = string.IsNullOrWhiteSpace(preConsultaViewModel.Numero_Carterinha) ? "" : preConsultaViewModel.Numero_Carterinha;
            preConsulta.Dt_Insert = DateTime.Now;

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            if (!_PreConsultaService.Update(ToModel(preConsulta), user_logado, out string resultado))
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

            if (!ReturnPermission(usuarioAtual, "Excluir Pré Atendimentos"))
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

                    ListaFiltrada = ListaFiltrada.Where(a => a.Agenda.Paciente.Nome.Contains(nome)).ToList();
                    preConsultaViewModel.Paciente_string = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["Convenio"]))
                {
                    var convenio = collection["Convenio"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Convenio.Contains(convenio)).ToList();
                    preConsultaViewModel.Convenio = convenio;
                }

            }

            var datenow = DateTime.Now.ToShortDateString();
            var date_string = DateTime.Now.ToString("yyyy-MM-dd");

            ListaFiltrada = ListaFiltrada.Where(a => a.Agenda.Data_Consulta.Equals(datenow)).ToList();
            preConsultaViewModel.DataDe_Filtro = date_string;

            preConsultaViewModel.ListPreConsultaViewModel = ListaFiltrada;

            return preConsultaViewModel;
        }

        #endregion

        #region Select List 

        public SelectList ListNomePacientes(object id = null)
        {
            List<string> list = new List<string>();

            var datenow = DateTime.Now.ToShortDateString();

            var ids_paciente = _AgendaService
                               .QueryAsNoTracking()
                               .Where(a => a.DATA_CONSULTA.Equals(datenow))
                               .Select(a => a.ID_PACIENTE)
                               .ToList();

            var pacientes = _PacienteService
                            .QueryAsNoTracking()
                            .Where(a => ids_paciente.Contains(a.ID))
                            .Select(a => a.NOME)
                            .ToList();

            list = pacientes.Distinct().OrderBy(a => a).ToList();

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

                //var pre_consultaList = ToListViewModel(_PreConsultaService.Query().Where(a => a.ID_PACIENTE == paciente.ID).ToList());

                //var pre_consulta = pre_consultaList.Count() > 0 ? pre_consultaList.OrderByDescending(a => a.Dt_Insert).FirstOrDefault() : new PreConsultaViewModel();

                //var existePreConsulta = pre_consulta.ID != 0 ? true : false;

                var dt_nasc = Convert.ToDateTime(paciente.Data_Nasc);

                int idade = DateTime.Now.Year - dt_nasc.Year;
                if (DateTime.Now.Month < dt_nasc.Month || (DateTime.Now.Month == dt_nasc.Month && DateTime.Now.Day < dt_nasc.Day))
                    idade--;

                var maiorIdade = idade >= 18 ? true : false;

                //if (existePreConsulta)
                //{
                //    if (maiorIdade)
                //    {
                //        return Json(new { Success = true, Response = "", Idade = idade, MaiorIdade = maiorIdade, PreConsulta = true,  });
                //    }
                //    else
                //    {
                //        return Json(new { Success = true, Response = "", Idade = idade, MaiorIdade = maiorIdade, PreConsulta = true, });
                //    }

                //}

                return Json(new { Success = true, Response = "", Idade = idade, MaiorIdade = maiorIdade });
            }
        }



        #endregion
    }
}