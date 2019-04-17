using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Repository.Model
{
    public class PreConsultaRepository : RepositoryBase<PreConsulta>, IPreConsultaRepository
    {
        public void UpdatePreConsulta(PreConsulta preConsulta, Paciente paciente)
        {
            var preConsultaUpdate = GetById(preConsulta.ID);
            try
            {
                preConsultaUpdate.PACIENTE = paciente;
                preConsultaUpdate.MAIOR_IDADE = preConsulta.MAIOR_IDADE;
                preConsultaUpdate.AUTORIZADO = preConsulta.AUTORIZADO;
                preConsultaUpdate.CONVENIO = preConsulta.CONVENIO;
                preConsultaUpdate.NUMERO_CARTERINHA = preConsulta.NUMERO_CARTERINHA;
                //preConsultaUpdate.VALOR_CONSULTA = preConsulta.VALOR_CONSULTA;
                //preConsultaUpdate.TIPO_PAGAMENTO = preConsulta.TIPO_PAGAMENTO;
                preConsultaUpdate.DT_INSERT = preConsulta.DT_INSERT;

                UpdateEntity(preConsultaUpdate);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert(PreConsulta preConsulta)
        {
            try
            {
                AddEntity(preConsulta);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(PreConsulta preConsulta)
        {
            try
            {
                base.DeleteEntity(preConsulta);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
