using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Repository.Model
{
    public class HistoricoAutorizacaoPacienteRepository : RepositoryBase<HistoricoAutorizacaoPaciente>, IHistoricoAutorizacaoPacienteRepository
    {
        public void InsertHistorico(int id_paciente, int id_assinatura)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString()))
                {
                    conn.Open();

                    var cmd = new SqlCommand("SP_INSERT_HISTORICO_AUTORIZACAO", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@fk_id_paciente", id_paciente));
                    cmd.Parameters.Add(new SqlParameter("@fk_id_assinatura", id_assinatura));

                    cmd.CommandTimeout = 0;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
