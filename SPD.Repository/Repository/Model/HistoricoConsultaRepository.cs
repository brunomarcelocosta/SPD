using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SPD.Repository.Repository.Model
{
    public class HistoricoConsultaRepository : RepositoryBase<HistoricoConsulta>, IHistoricoConsultaRepository
    {
        public void InsertHistorico(int id_consulta)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString()))
                {
                    conn.Open();

                    var cmd = new SqlCommand("SP_INSERT_HISTORICO_CONSULTA", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@fk_id_consulta", id_consulta));

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
