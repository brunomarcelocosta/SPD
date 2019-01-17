using SPD.Model.Model;
using SPD.MVC.Geral.Controllers;
using SPD.MVC.Geral.Utilities;
using SPD.MVC.PortalWeb.ViewModels;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static SPD.Model.Enums.SPD_Enums;

namespace SPD.MVC.PortalWeb.Controllers
{
    public class PacienteController : MapperController<IPacienteService, Paciente, PacienteViewModel>
    {
        private readonly IPacienteService _PacienteService;
        private readonly IUsuarioService _UsuarioService;

        public PacienteController(IPacienteService pacienteService, IUsuarioService usuarioService)
                   : base(pacienteService)
        {
            _PacienteService = pacienteService;
            _UsuarioService = usuarioService;
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
                    Data_Nasc = item.Data_Nasc.ToShortDateString(),
                    item.Cpf,
                    item.Celular,
                    tpPaciente = item.Tipo_Paciente == true ? "Particular" : "Convêniado",
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
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Tipo_Paciente).ToList() : data.OrderBy(p => p.Tipo_Paciente).ToList();
                        break;

                    case "8":
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

            return View(pacienteViewModel);
        }

        #endregion

        #region Editar

        [UseAuthorization(Funcionalidades = "{\"Nome\":\"Editar Usuários\"}")]
        public ActionResult Edit(int id)
        {
            PacienteViewModel pacienteViewModel = new PacienteViewModel();

            pacienteViewModel = ToViewModel(_PacienteService.GetById(id));
            pacienteViewModel.estadoCivil_String = pacienteViewModel.EstadoCivil.Descricao;

            return View(pacienteViewModel);
        }

        #endregion

        #region Excluir

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

                if (!string.IsNullOrWhiteSpace(collection["tipoPacienteFiltro"]))
                {
                    var status = collection["tipoPacienteFiltro"].ToString() == "true" ? true : false;

                    ListaFiltrada = ListaFiltrada.Where(a => a.Tipo_Paciente == status).ToList();
                    pacienteViewModel.tipoPacienteFiltro = status.ToString();
                }

                if (!string.IsNullOrWhiteSpace(collection["DataDe"]) && !string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Data_Nasc >= dataDe && a.Data_Nasc < dataAte.AddDays(1)).ToList();

                    pacienteViewModel.DataDe_Filtro = collection["DataDe"];
                    pacienteViewModel.DataAte_Filtro = collection["DataAte"];
                }

                else if (!string.IsNullOrWhiteSpace(collection["DataDe"]))
                {
                    var dataDe = Convert.ToDateTime(collection["DataDe"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Data_Nasc >= dataDe).ToList();
                    pacienteViewModel.DataDe_Filtro = collection["DataDe"];

                }

                else if (!string.IsNullOrWhiteSpace(collection["DataAte"]))
                {
                    var dataAte = Convert.ToDateTime(collection["DataAte"].ToString());

                    ListaFiltrada = ListaFiltrada.Where(a => a.Data_Nasc < dataAte.AddDays(1)).ToList();
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

        #endregion

        #region Webcam

        [HttpPost]
        public ActionResult Capture()
        {
            if (Request.InputStream.Length > 0)
            {
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    string hexString = Server.UrlEncode(reader.ReadToEnd());
                    string imageName = DateTime.Now.ToString("dd-MM-yy hh-mm-ss");
                    string imagePath = string.Format("~/Captures/{0}.png", imageName);
                    System.IO.File.WriteAllBytes(System.Web.HttpContext.Current.Server.MapPath(imagePath), ConvertHexToBytes(hexString));
                    Session["CapturedImage"] = VirtualPathUtility.ToAbsolute(imagePath);
                }
            }

            return View();
        }

        [HttpPost]
        public ContentResult GetCapture()
        {
            string url = Session["CapturedImage"].ToString();
            Session["CapturedImage"] = null;
            return Content(url);
        }

        private static byte[] ConvertHexToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        #endregion
    }
}