using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using SPD.Services.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Services.Services.Model
{
    public class AssinaturaService : ServiceBase<Assinatura>, IAssinaturaService
    {
        private readonly IAssinaturaRepository _AssinaturaRepository;
        public AssinaturaService(IAssinaturaRepository assinaturaRepository) : base(assinaturaRepository)
        {
            _AssinaturaRepository = assinaturaRepository;
        }
    }
}
