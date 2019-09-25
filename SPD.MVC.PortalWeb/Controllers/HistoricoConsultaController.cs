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
        public ActionResult List(int? page, string dt_init = "", string dt_end = "", string dentista = "", string paciente = "")
        {
            int pageNumber = (page ?? 1);
            int pageSize = 100;

            var list = ToListViewModel(_HistoricoConsultaService.Select().ToList());

            HistoricoConsultaViewModel historicoConsultaViewModel = new HistoricoConsultaViewModel
            {
                ListHistoricoConsultaViewModels = list,
                Paciente = paciente,
                Dentista = dentista,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            if (!string.IsNullOrWhiteSpace(dt_init))
            {
                historicoConsultaViewModel.DataDe = Convert.ToDateTime(dt_init);

                historicoConsultaViewModel.DataDe_Filtro = historicoConsultaViewModel.DataDe.Value.ToString("yyyy-MM-dd");
            }

            if (!string.IsNullOrWhiteSpace(dt_end))
            {
                historicoConsultaViewModel.DataAte = Convert.ToDateTime(dt_end);
                historicoConsultaViewModel.DataAte_Filtro = historicoConsultaViewModel.DataAte.Value.ToString("yyyy-MM-dd"); ;
            }

            historicoConsultaViewModel = Filtrar(historicoConsultaViewModel);

            historicoConsultaViewModel.ListHistoricoConsulta = historicoConsultaViewModel.ListHistoricoConsultaViewModels.ToPagedList(pageNumber, pageSize);
            historicoConsultaViewModel.PageCount = historicoConsultaViewModel.ListHistoricoConsultaViewModels.Count() / pageSize;

            return View(historicoConsultaViewModel);
        }

        public HistoricoConsultaViewModel Filtrar(HistoricoConsultaViewModel viewmodel)
        {
            List<HistoricoConsultaViewModel> ListaFiltrada = new List<HistoricoConsultaViewModel>();
            ListaFiltrada = viewmodel.ListHistoricoConsultaViewModels;


            if (!string.IsNullOrWhiteSpace(viewmodel.Paciente))
            {
                ListaFiltrada = ListaFiltrada.Where(a => a.Paciente.Contains(viewmodel.Paciente)).ToList();
            }

            if (!string.IsNullOrWhiteSpace(viewmodel.Dentista))
            {
                ListaFiltrada = ListaFiltrada.Where(a => a.Dentista.Contains(viewmodel.Dentista)).ToList();
            }

            if (viewmodel.DataDe.HasValue && viewmodel.DataAte.HasValue)
            {
                var dataDe = Convert.ToDateTime(viewmodel.DataDe.ToString());
                var dataAte = Convert.ToDateTime(viewmodel.DataAte.ToString());

                ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.DataConsulta) >= dataDe &&
                                                         Convert.ToDateTime(a.DataConsulta) < dataAte.AddDays(1))
                                             .ToList();

            }

            else if (viewmodel.DataDe.HasValue)
            {
                var dataDe = Convert.ToDateTime(viewmodel.DataDe.ToString());

                ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.DataConsulta) >= dataDe).ToList();
                viewmodel.DataDe_Filtro = viewmodel.DataDe_Filtro;

            }

            else if (viewmodel.DataAte.HasValue)
            {
                var dataAte = Convert.ToDateTime(viewmodel.DataAte.ToString());

                ListaFiltrada = ListaFiltrada.Where(a => Convert.ToDateTime(a.DataConsulta) < dataAte.AddHours(1)).ToList();
                viewmodel.DataAte_Filtro = viewmodel.DataAte_Filtro;
            }

            viewmodel.ListHistoricoConsultaViewModels = ListaFiltrada
                                                                        .OrderBy(a => a.DataConsulta)
                                                                        .ThenBy(a => a.Dentista)
                                                                        .ThenBy(a => a.Paciente)
                                                                        .ToList();

            return viewmodel;

        }

        #endregion
    }
}