using SPD.Model.Model;
using SPD.Model.Util;

namespace SPD.Services.Interface.Model
{
    public interface IAutenticacaoService : IServiceBase<Usuario>
    {
        [Transactional]
        bool AutenticarUsuario(ref Usuario usuario, string enderecoIP, string autenticacaoUrl, out string resultados);
    }
}
