using AutoMapper;
using SPD.Model.Model;
using SPD.MVC.PortalWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPD.MVC.PortalWeb.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappings"; }
        }

        public DomainToViewModelMappingProfile()
        {
            this.CreateMap<EstadoCivil, EstadoCivilViewModel>();
            this.CreateMap<Funcionalidade, FuncionalidadeViewModel>();
            this.CreateMap<HistoricoOperacao, HistoricoOperacaoViewModel>();
            this.CreateMap<Notificacao, NotificacaoViewModel>();
            this.CreateMap<Paciente, PacienteViewModel>();
            this.CreateMap<SessaoUsuario, SessaoUsuarioViewModel>();
            this.CreateMap<TipoOperacao, TipoOperacaoViewModel>();
            this.CreateMap<Usuario, UsuarioViewModel>();
            this.CreateMap<UsuarioFuncionalidade, UsuarioFuncionalidadeViewModel>();
        }
    }
}