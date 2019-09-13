using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class ConsultaController : MapperController<IConsultaService, Consulta, ConsultaViewModel>
    {
        private readonly IConsultaService _ConsultaService;
        private readonly IPreConsultaService _PreConsultaService;
        private readonly IDentistaService _DentistaService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IFuncionalidadeService _FuncionalidadeService;
        private readonly IUsuarioFuncionalidadeService _UsuarioFuncionalidadeService;
        private readonly IHistoricoConsultaService _HistoricoConsultaService;
        private readonly IPacienteService _PacienteService;

        public ConsultaController(IConsultaService consultaService,
                                  IPreConsultaService preconsultaService,
                                  IDentistaService dentistaService,
                                  IUsuarioService usuarioService,
                                  IFuncionalidadeService funcionalidadeService,
                                  IUsuarioFuncionalidadeService usuariofuncionalidadeService,
                                  IPacienteService pacienteService,
                                  IHistoricoConsultaService historicoConsultaService)
                : base(consultaService)
        {
            _ConsultaService = consultaService;
            _PreConsultaService = preconsultaService;
            _DentistaService = dentistaService;
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuariofuncionalidadeService;
            _HistoricoConsultaService = historicoConsultaService;
            _PacienteService = pacienteService;
        }

        #region List

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Consultas\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            ConsultaViewModel consultaViewModel = new ConsultaViewModel();

            var id_sessao = this.GetAuthenticationFromSession().ID;

            var dentista = _DentistaService.Query().Where(a => a.ID_USUARIO == id_sessao).FirstOrDefault();
            consultaViewModel.Dentista_string = dentista.NOME;

            consultaViewModel = ReturnConsulta(collection);

            return View(consultaViewModel);
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

            ConsultaViewModel consultaViewModel = new ConsultaViewModel();

            consultaViewModel = ReturnConsulta(collection);

            int totalRecords = consultaViewModel.ListPreConsultaViewModel.Count;

            int recFilter = consultaViewModel.ListPreConsultaViewModel.Count;

            consultaViewModel.ListPreConsultaViewModel = consultaViewModel.ListPreConsultaViewModel.Skip(startRec).Take(pageSize).ToList();

            List<object> listToView = new List<object>();

            foreach (var item in consultaViewModel.ListPreConsultaViewModel.OrderBy(a => a.Agenda.Dentista.Nome).ThenBy(a => a.Agenda.Hora_Inicio))
            {
                var existe = _ConsultaService.QueryAsNoTracking().Where(a => a.ID_PRE_CONSULTA == item.ID).ToList().Count() > 0 ? true : false;


                listToView.Add(new
                {
                    item.ID,
                    Hora = item.Agenda.Hora_Inicio,
                    Paciente = item.Agenda.Nome_Paciente,
                    Autorizado = item.Autorizado == true ? "Sim" : "Não",
                    item.Convenio,
                    existe
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

        public ConsultaViewModel ReturnConsulta(FormCollection collection = null)
        {
            ConsultaViewModel consultaViewModel = new ConsultaViewModel
            {
                ListPreConsultaViewModel = ToListViewModel<PreConsulta, PreConsultaViewModel>(_PreConsultaService
                                                                                              .QueryAsNoTracking()
                                                                                              .Where(a => a.CONSULTA_INICIADA == false)
                                                                                              .ToList()
                                                                                             )
            };

            consultaViewModel = Filtrar(consultaViewModel, collection);

            return consultaViewModel;
        }

        #endregion

        #region Filtrar

        public ConsultaViewModel Filtrar(ConsultaViewModel consultaViewModel, FormCollection collection)
        {
            List<PreConsultaViewModel> ListaFiltrada = new List<PreConsultaViewModel>();

            ListaFiltrada = consultaViewModel.ListPreConsultaViewModel;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Paciente_string"]))
                {
                    var nome = collection["Paciente_string"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Agenda.Nome_Paciente.Contains(nome)).ToList();
                    consultaViewModel.Paciente_string = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["HoraDe_Filtro"]) && !string.IsNullOrWhiteSpace(collection["HoraDe_Filtro"]))
                {
                    var dataDe = Convert.ToDateTime(collection["HoraDe_Filtro"].ToString());
                    var dataAte = Convert.ToDateTime(collection["HoraAte_Filtro"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Agenda.Hora_Inicio) >= dataDe &&
                                                             Convert.ToDateTime(a.Agenda.Hora_Fim) < dataAte.AddHours(1))
                                                 .ToList();

                    consultaViewModel.DataDe_Filtro = collection["HoraDe_Filtro"];
                    consultaViewModel.DataAte_Filtro = collection["HoraAte_Filtro"];
                }

                else if (!string.IsNullOrWhiteSpace(collection["HoraDe_Filtro"]))
                {
                    var dataDe = Convert.ToDateTime(collection["HoraDe_Filtro"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Agenda.Hora_Inicio) >= dataDe).ToList();
                    consultaViewModel.DataDe_Filtro = collection["HoraDe_Filtro"];

                }

                else if (!string.IsNullOrWhiteSpace(collection["HoraAte_Filtro"]))
                {
                    var dataAte = Convert.ToDateTime(collection["HoraAte_Filtro"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.Agenda.Hora_Fim) < dataAte.AddHours(1)).ToList();
                    consultaViewModel.DataAte_Filtro = collection["HoraAte_Filtro"];
                }
            }

            var datenow = DateTime.Now.ToShortDateString();
            var date_string = DateTime.Now.ToString("yyyy-MM-dd");

            ListaFiltrada = ListaFiltrada.Where(a => a.Agenda.Data_Consulta.Equals(datenow)).ToList();
            consultaViewModel.DataDe_Filtro = date_string;

            var id_sessao = this.GetAuthenticationFromSession().ID;
            var dentista = _DentistaService.Query().Where(a => a.ID_USUARIO == id_sessao).FirstOrDefault().NOME;

            ListaFiltrada = ListaFiltrada.Where(a => a.Agenda.Dentista.Nome.Contains(dentista)).ToList();
            consultaViewModel.Dentista_string = dentista;

            consultaViewModel.ListPreConsultaViewModel = ListaFiltrada;

            return consultaViewModel;
        }

        #endregion

        #region Novo

        public ActionResult New(string id)
        {
            var preConsulta = ToViewModel<PreConsulta, PreConsultaViewModel>(_PreConsultaService.GetById(int.Parse(id)));

            var id_sessao = this.GetAuthenticationFromSession().ID;
            var dentista = _DentistaService.Query().Where(a => a.ID_USUARIO == id_sessao).FirstOrDefault();

            var id_paciente = preConsulta.Agenda.ID_Paciente;
            var paciente = ToViewModel<Paciente, PacienteViewModel>(_PacienteService.GetById(id_paciente.Value));

            var historicos = ToListViewModel<HistoricoConsulta, HistoricoConsultaViewModel>
                            (
                                _HistoricoConsultaService
                                .Query()
                                .Where(a => a.CONSULTA.ID_DENTISTA == dentista.ID &&
                                            a.CONSULTA.PRE_CONSULTA.AGENDA.ID_PACIENTE == id_paciente
                                       )
                                .ToList()
                            );

            var idade = ReturnIdade(paciente.Data_Nasc, out bool maior_idade);
            var odontograma = GetOdontograma(maior_idade);
            var odontograma_string = $"data:image/png;base64,{Convert.ToBase64String(odontograma)}";

            ConsultaViewModel consultaViewModel = new ConsultaViewModel
            {
                ID_Pre_Consulta = preConsulta.ID,
                ID_Dentista = dentista.ID,
                Paciente_string = preConsulta.Agenda.Nome_Paciente,
                Celular = preConsulta.Agenda.Celular,
                Idade_string = $"{idade} anos",
                ListHistoricoConsultaViewModels = historicos,
                Convenio_string = preConsulta.Convenio,
                Img_string = odontograma_string
            };

            return View(consultaViewModel);
        }

        public ActionResult Add(ConsultaViewModel consultaViewModel)
        {
            var img = consultaViewModel.Img_string.Substring("data:image/png;base64,".Length);

            var dentista = _DentistaService.GetById(consultaViewModel.ID_Dentista);
            var preConsulta = _PreConsultaService.GetById(consultaViewModel.ID_Pre_Consulta);

            var consulta = new Consulta()
            {
                DENTISTA = dentista,
                PRE_CONSULTA = preConsulta,
                DESCRICAO_PROCEDIMENTO = consultaViewModel.Descricao_Procedimento,
                ODONTOGRAMA = Convert.FromBase64String(img),
                DT_CONSULTA = DateTime.Now
            };

            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            if (!_ConsultaService.Insert(consulta, preConsulta.AGENDA.NOME_PACIENTE, user_logado, out string resultado))
            {
                return Json(new
                {
                    Success = false,
                    Response = resultado
                });
            }

            return Json(new { Success = true });
        }
        #endregion

        #region Validações

        public ActionResult ValidaDentista()
        {
            var user_logado = _UsuarioService.GetById(this.GetAuthenticationFromSession().ID);

            if (!ReturnPermission(user_logado, "Iniciar Consulta"))
            {
                return Json(new { Success = false, Response = "Você não tem permissão para esta funcionalidade." });
            }

            var dentista = _DentistaService.Query().Where(a => a.ID_USUARIO == user_logado.ID).FirstOrDefault();

            if (dentista == null)
            {
                return Json(new { Success = false, Response = "É necessário ser um dentista." });
            }

            return Json(new { Success = true });
        }

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

        #region Gets

        public byte[] GetOdontograma(bool maior_idade)
        {
            var filePath = maior_idade ? ConfigurationManager.AppSettings["odontograma"].ToString() : ConfigurationManager.AppSettings["odontogramaInfantil"].ToString();

            FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            byte[] imagemArray = br.ReadBytes((int)fs.Length);

            br.Close();
            fs.Close();

            return imagemArray;
        }

        #endregion
    }
}