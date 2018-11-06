using AutoMapper;
using SPD.MVC.Geral.ViewModels;
using System.Collections.Generic;
using System.Web.Mvc;
using SPD.Services.Interface;
using SPD.MVC.Geral.Util;

namespace SPD.MVC.Geral.Controllers
{
    /// <summary>
    /// Classe de controller com funcionalidade de conversão entre models e viewmodels.
    /// </summary>
    public class MapperController<TSource, TModel, TViewModel> : SecurityController
        where TSource : IServiceBase<TModel>
        where TModel : class
        where TViewModel : ViewModelBase
    {
        public TSource ApplicationService { get; }

        protected MapperController(TSource applicationService)
        {
            this.ApplicationService = applicationService;
        }


        #region Singular

        protected TModel ToModel(TViewModel source)
        {
            return Mapper.Map<TViewModel, TModel>(source);
        }

        protected TViewModel ToViewModel(TModel source)
        {
            return Mapper.Map<TModel, TViewModel>(source);
        }

        protected TInternalModel ToModel<TInternalViewModel, TInternalModel>(TInternalViewModel source)
        {
            return Mapper.Map<TInternalViewModel, TInternalModel>(source);
        }

        protected TInternalViewModel ToViewModel<TInternalModel, TInternalViewModel>(TInternalModel source)
        {
            return Mapper.Map<TInternalModel, TInternalViewModel>(source);
        }

        #endregion

        #region Collections: In IEnumerable<> Out IEnumerable<>

        protected IEnumerable<TModel> ToEnumerableModel(IEnumerable<TViewModel> source)
        {
            return Mapper.Map<IEnumerable<TViewModel>, IEnumerable<TModel>>(source);
        }

        protected IEnumerable<TViewModel> ToEnumerableViewModel()
        {
            return Mapper.Map<IEnumerable<TModel>, IEnumerable<TViewModel>>(this.ApplicationService.GetAll());
        }

        protected IEnumerable<TViewModel> ToEnumerableViewModel(IEnumerable<TModel> source)
        {
            return Mapper.Map<IEnumerable<TModel>, IEnumerable<TViewModel>>(source);
        }

        #endregion

        #region Collections: In ICollection<> Out IEnumerable<>

        protected IEnumerable<TModel> ToEnumerableModel(ICollection<TViewModel> source)
        {
            return Mapper.Map<ICollection<TViewModel>, IEnumerable<TModel>>(source);
        }

        protected IEnumerable<TViewModel> ToEnumerableViewModel(ICollection<TModel> source)
        {
            return Mapper.Map<ICollection<TModel>, IEnumerable<TViewModel>>(source);
        }

        #endregion

        #region Collections: In IEnumerable<> Out List<>

        protected List<TModel> ToListModel(IEnumerable<TViewModel> source)
        {
            return Mapper.Map<IEnumerable<TViewModel>, List<TModel>>(source);
        }

        protected List<TViewModel> ToListViewModel()
        {
            return Mapper.Map<IEnumerable<TModel>, List<TViewModel>>(this.ApplicationService.GetAll());
        }

        protected List<TViewModel> ToListViewModel(IEnumerable<TModel> source)
        {
            return Mapper.Map<IEnumerable<TModel>, List<TViewModel>>(source);
        }

        #endregion

        #region Collections: In ICollection<> Out List<>

        protected List<TModel> ToListModel(ICollection<TViewModel> source)
        {
            return Mapper.Map<ICollection<TViewModel>, List<TModel>>(source);
        }

        protected List<TViewModel> ToListViewModel(ICollection<TModel> source)
        {
            return Mapper.Map<ICollection<TModel>, List<TViewModel>>(source);
        }

        #endregion

        #region Collections: In IEnumerable<TInternalModel, TInternalViewModel> Out IEnumerable<TInternalModel, TInternalViewModel>

        protected IEnumerable<TInternalModel> ToEnumerableModel<TInternalModel, TInternalViewModel>(IEnumerable<TInternalViewModel> source)
        {
            return Mapper.Map<IEnumerable<TInternalViewModel>, IEnumerable<TInternalModel>>(source);
        }

        protected IEnumerable<TInternalViewModel> ToEnumerableViewModel<TInternalModel, TInternalViewModel>(IEnumerable<TInternalModel> source)
        {
            return Mapper.Map<IEnumerable<TInternalModel>, IEnumerable<TInternalViewModel>>(source);
        }

        #endregion

        #region Collections: In ICollection<TInternalModel, TInternalViewModel> Out IEnumerable<TInternalModel, TInternalViewModel>

        protected IEnumerable<TInternalModel> ToEnumerableModel<TInternalModel, TInternalViewModel>(ICollection<TInternalViewModel> source)
        {
            return Mapper.Map<ICollection<TInternalViewModel>, IEnumerable<TInternalModel>>(source);
        }

        protected IEnumerable<TInternalViewModel> ToEnumerableViewModel<TInternalModel, TInternalViewModel>(ICollection<TInternalModel> source)
        {
            return Mapper.Map<ICollection<TInternalModel>, IEnumerable<TInternalViewModel>>(source);
        }

        #endregion

        #region Collections: In IEnumerable<TInternalModel, TInternalViewModel> Out List<TInternalModel, TInternalViewModel>

        protected List<TInternalModel> ToListModel<TInternalModel, TInternalViewModel>(IEnumerable<TInternalViewModel> source)
        {
            return Mapper.Map<IEnumerable<TInternalViewModel>, List<TInternalModel>>(source);
        }

        protected List<TInternalViewModel> ToListViewModel<TInternalModel, TInternalViewModel>(IEnumerable<TInternalModel> source)
        {
            return Mapper.Map<IEnumerable<TInternalModel>, List<TInternalViewModel>>(source);
        }

        #endregion

        #region Collections: In ICollection<TInternalModel, TInternalViewModel> Out List<TInternalModel, TInternalViewModel>

        protected List<TInternalModel> ToListModel<TInternalModel, TInternalViewModel>(ICollection<TInternalViewModel> source)
        {
            return Mapper.Map<ICollection<TInternalViewModel>, List<TInternalModel>>(source);
        }

        protected List<TInternalViewModel> ToListViewModel<TInternalModel, TInternalViewModel>(ICollection<TInternalModel> source)
        {
            return Mapper.Map<ICollection<TInternalModel>, List<TInternalViewModel>>(source);
        }

        #endregion

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Config = LeituraArquivo.LerArquivoConfig();
            ViewBag.Menu = LeituraArquivo.LerArquivoMenu();

            var mapperController = filterContext.Controller as MapperController<TSource, TModel, TViewModel>;

            mapperController.Response.AppendHeader("Access-Control-Allow-Origin", "*");

            var authenticationViewModel = this.GetAuthenticationFromSession();

            mapperController.ApplicationService.Context = this.SetupContext(authenticationViewModel.ID, authenticationViewModel.EnderecoIP);

            base.OnActionExecuting(filterContext);
        }
    }
}
