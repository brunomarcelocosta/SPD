using SPD.Model.Model;
using SPD.Repository.Interface.Model;
using System;

namespace SPD.Repository.Repository.Model
{
    public class PacienteRepository : RepositoryBase<Paciente>, IPacienteRepository
    {
        public void UpdatePaciente(Paciente paciente)
        {
            try
            {
                var pacienteUpdate = GetById(paciente.ID);

                pacienteUpdate.CPF = paciente.CPF;
                pacienteUpdate.NOME = paciente.NOME;
                pacienteUpdate.DATA_NASC = paciente.DATA_NASC;
                pacienteUpdate.RG = paciente.RG;
                pacienteUpdate.ATIVO = paciente.ATIVO;
                pacienteUpdate.EMAIL = paciente.EMAIL;
                pacienteUpdate.CELULAR = paciente.CELULAR;
                pacienteUpdate.ESTADO_CIVIL = paciente.ESTADO_CIVIL;
                pacienteUpdate.PROFISSAO = paciente.PROFISSAO;
                pacienteUpdate.FOTO = paciente.FOTO;
                pacienteUpdate.END_RUA = paciente.END_RUA;
                pacienteUpdate.END_NUMERO = paciente.END_NUMERO;
                pacienteUpdate.END_COMPL = paciente.END_COMPL;
                pacienteUpdate.BAIRRO = paciente.BAIRRO;
                pacienteUpdate.CEP = paciente.CEP;
                pacienteUpdate.CIDADE = paciente.CIDADE;
                pacienteUpdate.UF = paciente.UF;
                pacienteUpdate.PAIS = paciente.PAIS;

                UpdateEntity(pacienteUpdate);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert(Paciente paciente)
        {
            try
            {
                AddEntity(paciente);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(Paciente paciente)
        {
            try
            {
                base.DeleteEntity(paciente);

                SaveChange();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
