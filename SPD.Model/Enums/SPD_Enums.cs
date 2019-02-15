using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Model.Enums
{
    public class SPD_Enums
    {
        public enum Paginas
        {

        }

        public enum TipoFiltro
        {

        }

        public enum Direcao
        {
            None,
            Ascending,
            Descending,
        }

        public enum Tipo_Operacao
        {
            Inclusao = 1,
            Alteracao = 2,
            Exclusao = 3,
            Consulta = 4,
            Impressao = 5,
            Exportacao = 6,
            Login = 7,
            Logoff = 8,
            Sistema = 9,
            Senha = 12
        }

        public enum Tipo_Funcionalidades
        {
            EfetuarLogin = 1,
            EfetuarLogof = 2,
            Usuarios = 3,
            Pacientes = 4,
            Dentista = 5,
            Consulta = 6,
            PreConsulta = 7
        }

        public enum EstadosCivis
        {
            Solteiro = 1,
            Casado = 2,
            Divorciado = 3,
            Separado = 4,
            Viúvo = 5,
            Nenhum = 6
        }

        public enum Propagation
        {
            /// <summary>
            /// A transaction is required by the scope. It uses an ambient transaction if one already exists. Otherwise, it creates a new transaction before entering the scope. This is the default value.
            /// </summary>
            Required,

            /// <summary>
            /// A new transaction is always created for the scope.
            /// </summary>
            RequiresNew,

            /// <summary>
            /// The ambient transaction context is suppressed when creating the scope. All operations within the scope are done without an ambient transaction context.
            /// </summary>
            Suppress
        }

    }
}
