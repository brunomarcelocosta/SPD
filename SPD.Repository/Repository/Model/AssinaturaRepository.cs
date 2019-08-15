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
                try
                {
                    AddEntity(assinatura);
                    SaveChange();

                    return GetById(assinatura.ID);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                var mesma_assinatura = QueryAsNoTracking().Where(a => a.ASSINATURA == assinatura.ASSINATURA).FirstOrDefault();

                if (mesma_assinatura == null)
                {
                    var id = QueryAsNoTracking()
                              .Where(a => a.CPF_RESPONSAVEL == assinatura.CPF_RESPONSAVEL)
                              .FirstOrDefault();
                    id.NOME_RESPONSAVEL = assinatura.NOME_RESPONSAVEL;
                    id.ASSINATURA = assinatura.ASSINATURA;

                    UpdateAssinatura(id);
                }
            }

            assinaturaReturn = QueryAsNoTracking()
                              .Where(a => a.CPF_RESPONSAVEL == assinatura.CPF_RESPONSAVEL
                                       && a.NOME_RESPONSAVEL == assinatura.NOME_RESPONSAVEL)
                              .FirstOrDefault();

            return assinaturaReturn;
        }

        public void UpdateAssinatura(Assinatura assinatura)
        {
            var assUpdate = GetById(assinatura.ID);

            try
            {
                assUpdate.ASSINATURA = assinatura.ASSINATURA;
                assUpdate.NOME_RESPONSAVEL = assinatura.NOME_RESPONSAVEL;

                UpdateEntity(assUpdate);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
