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
    public class ConsultaController : MapperController<IConsultaService, Consulta, ConsultaViewModel>
    {
        private readonly IConsultaService _ConsultaService;
        private readonly IDentistaService _DentistaService;
        private readonly IUsuarioService _UsuarioService;
        private readonly IFuncionalidadeService _FuncionalidadeService;
        private readonly IUsuarioFuncionalidadeService _UsuarioFuncionalidadeService;

        public ConsultaController(IConsultaService consultaService,
                                  IDentistaService dentistaService,
                                  IUsuarioService usuarioService,
                                  IFuncionalidadeService funcionalidadeService,
                                  IUsuarioFuncionalidadeService usuariofuncionalidadeService)
                : base(consultaService)
        {
            _ConsultaService = consultaService;
            _DentistaService = dentistaService;
            _UsuarioService = usuarioService;
            _FuncionalidadeService = funcionalidadeService;
            _UsuarioFuncionalidadeService = usuariofuncionalidadeService;
        }

        #region List

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Listar Consultas\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            ConsultaViewModel consultaViewModel = new ConsultaViewModel();

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

        public ConsultaViewModel ReturnConsulta(FormCollection collection = null)
        {
            ConsultaViewModel consultaViewModel = new ConsultaViewModel
            {
                // ListPreConsultaViewModel = ToListViewModel(_PreConsultaService.QueryAsNoTracking().ToList())
            };

            consultaViewModel = Filtrar(consultaViewModel, collection);

            return consultaViewModel;
        }

        #endregion

        #region Filtrar

        public ConsultaViewModel Filtrar(ConsultaViewModel consultaViewModel, FormCollection collection)
        {
            List<ConsultaViewModel> ListaFiltrada = new List<ConsultaViewModel>();

            ListaFiltrada = consultaViewModel.ListConsultaViewModel;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Paciente_string"]))
                {
                    var nome = collection["Paciente_string"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Pre_Consulta.Agenda.Nome_Paciente.Contains(nome)).ToList();
                    consultaViewModel.Paciente_string = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["HoraDe"]) && !string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataDe = Convert.ToDateTime(collection["HoraDe"].ToString());
                    var dataAte = Convert.ToDateTime(collection["HoraAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Pre_Consulta.Agenda.Hora_Inicio >= dataDe &&
                                                             a.Pre_Consulta.Agenda.Hora_Fim < dataAte.AddHours(1))
                                                 .ToList();

                    consultaViewModel.DataDe_Filtro = collection["HoraDe"];
                    consultaViewModel.DataAte_Filtro = collection["HoraAte"];
                }

                else if (!string.IsNullOrWhiteSpace(collection["HoraDe"]))
                {
                    var dataDe = Convert.ToDateTime(collection["HoraDe"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Pre_Consulta.Agenda.Hora_Inicio >= dataDe).ToList();
                    consultaViewModel.DataDe_Filtro = collection["HoraDe"];

                }

                else if (!string.IsNullOrWhiteSpace(collection["HoraAte"]))
                {
                    var dataAte = Convert.ToDateTime(collection["HoraAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Pre_Consulta.Agenda.Hora_Fim < dataAte.AddHours(1)).ToList();
                    consultaViewModel.DataAte_Filtro = collection["HoraAte"];
                }
            }

            var datenow = DateTime.Now.ToShortDateString();
            var date_string = DateTime.Now.ToString("yyyy-MM-dd");

            ListaFiltrada = ListaFiltrada.Where(a => a.Pre_Consulta.Agenda.Data_Consulta.Equals(datenow)).ToList();
            consultaViewModel.DataDe_Filtro = date_string;

            consultaViewModel.ListConsultaViewModel = ListaFiltrada;

            return consultaViewModel;
        }

        #endregion
    }
}