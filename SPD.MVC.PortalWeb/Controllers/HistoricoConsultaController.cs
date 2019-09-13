using System;
using System.Collections.Generic;
using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System.Linq;
using System.Web.Mvc;
using PagedList;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class HistoricoConsultaController : MapperController<IHistoricoConsultaService, HistoricoConsulta, HistoricoConsultaViewModel>
    {
        private readonly IHistoricoConsultaService _HistoricoConsultaService;
        private readonly IConsultaService _ConsultaService;
        private readonly IPreConsultaService _PreConsultaService;
        private readonly IDentistaService _DentistaService;


        public HistoricoConsultaController(IHistoricoConsultaService historicoConsultaService,
                                           IConsultaService consultaService,
                                           IPreConsultaService preconsultaService,
                                           IDentistaService dentistaService
                                  )
             : base(historicoConsultaService)
        {
            _HistoricoConsultaService = historicoConsultaService;
            _ConsultaService = consultaService;
            _PreConsultaService = preconsultaService;
            _DentistaService = dentistaService;
        }

        #region List

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Visualizar Histórico de Operação\"}")]
        public ActionResult List(FormCollection collection = null)
        {
            int pageSize = 50;
            int pagedIndex = 1;

            IPagedList<HistoricoConsultaViewModel> paged = null;

            var list = new List<HistoricoConsultaViewModel>();


            foreach (var item in ToListViewModel(_HistoricoConsultaService.QueryAsNoTracking().ToList()))
            {
                //item.Consulta.Pre_Consulta = ToViewModel<PreConsulta, PreConsultaViewModel>(_PreConsultaService.GetById(item.Consulta.ID_Pre_Consulta));
                var model = new HistoricoConsultaViewModel()
                {
                    DescricaoConsulta = item.Consulta.Descricao_Procedimento,
                    DataConsulta = item.Consulta.Pre_Consulta.Agenda.Data_Consulta,
                    Paciente = item.Consulta.Pre_Consulta.Agenda.Nome_Paciente,
                    Dentista = item.Consulta.Dentista.Nome
                };

                list.Add(model);
            }

            HistoricoConsultaViewModel historicoConsultaViewModel = new HistoricoConsultaViewModel
            {
                ListHistoricoConsultaViewModels = list
                                                                        .OrderBy(a => a.DataConsulta)
                                                                        .ThenBy(a => a.Dentista)
                                                                        .ThenBy(a => a.Paciente)
                                                                        .ToList()
            };

            historicoConsultaViewModel = Filtrar(historicoConsultaViewModel, collection);

            paged = historicoConsultaViewModel.ListHistoricoConsultaViewModels.ToPagedList(pagedIndex, pageSize);

            return View(historicoConsultaViewModel);
        }

        public HistoricoConsultaViewModel Filtrar(HistoricoConsultaViewModel historicoConsultaViewModel, FormCollection collection)
        {
            List<HistoricoConsultaViewModel> ListaFiltrada = new List<HistoricoConsultaViewModel>();
            ListaFiltrada = historicoConsultaViewModel.ListHistoricoConsultaViewModels;

            if (collection != null)
            {
                if (!string.IsNullOrWhiteSpace(collection["Paciente"]))
                {
                    var nome = collection["Paciente"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Paciente.Contains(nome)).ToList();
                    historicoConsultaViewModel.Paciente = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["Dentista"]))
                {
                    var nome = collection["Dentista"].ToString();

                    ListaFiltrada = ListaFiltrada.Where(a => a.Dentista.Contains(nome)).ToList();
                    historicoConsultaViewModel.Dentista = nome;
                }

                if (!string.IsNullOrWhiteSpace(collection["DataDe"]) && !string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.DataConsulta) >= dataDe &&
                                                             Convert.ToDateTime(a.DataConsulta) < dataAte.AddDays(1))
                                                 .ToList();

                    historicoConsultaViewModel.DataDe_Filtro = collection["DataDe"];
                    historicoConsultaViewModel.DataAte_Filtro = collection["DataAte"];
                }

                else if (!string.IsNullOrWhiteSpace(collection["DataDe"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.DataConsulta) >= dataDe).ToList();
                    historicoConsultaViewModel.DataDe_Filtro = collection["DataDe"];

                }

                else if (!string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.DataConsulta) < dataAte.AddHours(1)).ToList();
                    historicoConsultaViewModel.DataAte_Filtro = collection["DataAte"];
                }

                historicoConsultaViewModel.ListHistoricoConsultaViewModels = ListaFiltrada;
            }

            return historicoConsultaViewModel;

        }

        #endregion
    }
}