using AutoMapper;
using SPD.Model.Model;
using SPD.MVC.PortalWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappings"; }
        }

        public ViewModelToDomainMappingProfile()
        {
            this.CreateMap<FuncionalidadeViewModel, Funcionalidade>();
            this.CreateMap<HistoricoOperacaoViewModel, HistoricoOperacao>();
            this.CreateMap<NotificacaoViewModel, Notificacao>();
            this.CreateMap<SessaoUsuarioViewModel, SessaoUsuario>();
            this.CreateMap<TipoOperacaoViewModel, TipoOperacao>();
            this.CreateMap<UsuarioViewModel, Usuario>();
            this.CreateMap<UsuarioFuncionalidadeViewModel, UsuarioFuncionalidade>();
        }
    }
}