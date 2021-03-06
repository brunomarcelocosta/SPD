﻿using SPD.Model.Model;
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
        public void InsertHistorico(int id_paciente, byte[] assinatura, string nome, string cpf, out int id_assinatura)
        {
            try
            {
                id_assinatura = 0;

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlServer"].ToString()))
                {
                    conn.Open();

                    var cmd = new SqlCommand("SP_INSERT_HISTORICO_AUTORIZACAO", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };

                    cmd.Parameters.Add(new SqlParameter("@fk_id_paciente", id_paciente));
                    cmd.Parameters.Add(new SqlParameter("@assinatura", assinatura));
                    cmd.Parameters.Add(new SqlParameter("@nome", nome));
                    cmd.Parameters.Add(new SqlParameter("@cpf", cpf));

                    cmd.CommandTimeout = 0;

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        id_assinatura = reader[0] == null ? 0 : Convert.ToInt32(reader[0].ToString());
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
