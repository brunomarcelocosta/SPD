using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SPD.Repository.Repository.Model
{
    public class ConsultaRepository : RepositoryBase<Consulta>, IConsultaRepository
    {
        public void Insert(Consulta consulta)
        {
            try
            {
                AddEntity(consulta);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void InsertSQL(Consulta consulta)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString()))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_INSERT_CONSULTA", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    })
                    {
                        cmd.Parameters.Add(new SqlParameter("@fk_id_dentista", consulta.DENTISTA.ID));
                        cmd.Parameters.Add(new SqlParameter("@fk_id_pre_consulta", consulta.PRE_CONSULTA.ID));
                        cmd.Parameters.Add(new SqlParameter("@desc_procedimento", consulta.DESCRICAO_PROCEDIMENTO));
                        cmd.Parameters.Add(new SqlParameter("@odontograma", consulta.ODONTOGRAMA));
                        //cmd.Parameters.Add(new SqlParameter("@exame", consulta.EXAME ?? null));

                        cmd.CommandTimeout = 0;

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
