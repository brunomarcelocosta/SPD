using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Repository.Model
{
    public class AssinaturaRepository : RepositoryBase<Assinatura>, IAssinaturaRepository
    {

        public Assinatura GetAssinatura(Assinatura assinatura, bool existe)
        {
            var assinaturaReturn = new Assinatura();

            if (!existe)
            {
                Add(assinatura);
                SaveChanges();

                return GetById(assinatura.ID);

            }

            assinaturaReturn = Query()
                              .Where(a => a.CPF_RESPONSAVEL == assinatura.CPF_RESPONSAVEL 
                                       && a.NOME_RESPONSAVEL == assinatura.NOME_RESPONSAVEL)
                              .FirstOrDefault();

            return assinaturaReturn;
        }

    }
}
