using AutoMapper;

namespace SPD.MVC.Geral.AutoMapper
{
    public class AutoMapperConfiguration
    {
        public static void RegisterMappings<DomainToViewModel, ViewModelToDomain>()
            where DomainToViewModel : Profile, new()
            where ViewModelToDomain : Profile, new()
        {
            Mapper.Initialize(mapper =>
            {
                mapper.AddProfile<DomainToViewModel>();
                mapper.AddProfile<ViewModelToDomain>();
            });
        }
    }
}
