using System;
using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;
using System.Globalization;

namespace SPD.Data.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DatabaseInterceptor : IDbCommandInterceptor
    {
        /// <summary>
        /// Método que foi executado de uma consulta ao banco de dados
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", DateTime.Now, Environment.NewLine, command.CommandText, Environment.NewLine));
        }

        /// <summary>
        /// Método que está em execução de uma consulta ao banco de dados
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", DateTime.Now, Environment.NewLine, command.CommandText, Environment.NewLine));
        }

        /// <summary>
        /// Método que para ler o qeu foi executado no banco de dados
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", DateTime.Now, Environment.NewLine, command.CommandText, Environment.NewLine));
        }

        /// <summary>
        /// Método que para ler o qeu está executando no banco de dados
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", DateTime.Now, Environment.NewLine, command.CommandText, Environment.NewLine));
        }

        /// <summary>
        /// Método scalar que foi executado
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", DateTime.Now, Environment.NewLine, command.CommandText, Environment.NewLine));
        }

        /// <summary>
        /// Método scalar que está executando
        /// </summary>
        /// <param name="command"></param>
        /// <param name="interceptionContext"></param>
        public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0}{1}{2}{3}", DateTime.Now, Environment.NewLine, command.CommandText, Environment.NewLine));
        }
    }
}
