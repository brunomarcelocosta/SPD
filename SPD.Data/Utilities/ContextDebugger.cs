using SPD.Data.Content.Texts;
using System;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Globalization;

namespace SPD.Data.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ContextDebugger
    {
        /// <summary>
        /// Método responsável por escrever na linha de debug do projeto a exeção gerada pela validação do Entity
        /// </summary>
        /// <param name="dbEntityValidationException"></param>
        public static void ShowInDebugConsole(DbEntityValidationException dbEntityValidationException)
        {
            foreach (var entityValidationErrors in dbEntityValidationException.EntityValidationErrors)
            {
                Debug.WriteLine(
                    ErrorResource.DatabaseInitializerValidationError,
                    entityValidationErrors.Entry.Entity.GetType().Name,
                    entityValidationErrors.Entry.State
                );

                foreach (var validationErrors in entityValidationErrors.ValidationErrors)
                {
                    Debug.WriteLine(
                        ErrorResource.DatabaseInitializerPropertyValidationError,
                        validationErrors.PropertyName,
                        validationErrors.ErrorMessage
                    );
                }
            }
        }

        /// <summary>
        /// Método responsável por escrever na linha de debug do projeto a exeção gerada
        /// </summary>
        /// <param name="exception"></param>
        public static void ShowInDebugConsole(Exception exception)
        {
            Debug.WriteLine(String.Format(CultureInfo.InvariantCulture, "{0}: {1}", DateTime.Now, exception.Message));

            if (exception.InnerException != null)
            {
                ContextDebugger.ShowInDebugConsole(exception.InnerException);
            }
        }
    }
}
