using SPD.Data.Interfaces.Contexts;
using SPD.Data.Utilities;
using System;
using System.Data.Entity;
using SPD.Model.Model;

namespace SPD.Data.Contexts
{
    public sealed class DomainContext : IDisposable
    {
        private dynamic _context;

        /// <summary>
        /// Return COntext dynamic
        /// </summary>
        public dynamic Context
        {
            get { return _context; }
            set
            {
                _context = value;
                this.DataContext.Context = value;
            }
        }

        public bool IsDisposable { get; set; }
        public IDataContext DataContext { get; }

        public DbSet<EstadoCivil> EstadoCivil { get; set; }
        public DbSet<Funcionalidade> Funcionalidades { get; set; }
        public DbSet<HistoricoOperacao> HistoricoOperacoes { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<SessaoUsuario> SessaoUsuario { get; set; }
        public DbSet<TipoOperacao> TipoOperacao { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<UsuarioFuncionalidade> UsuarioFuncionalidades { get; set; }

        public DomainContext(ContextType contextType, bool isDisposable = true)
        {
            this.IsDisposable = isDisposable;

            if (this.IsDisposable)
            {
                switch (contextType)
                {

                    case ContextType.SqlServer:
                        this.DataContext = ContextProvider.GetDisposableInstance<SqlServerContext>();
                        break;

                    default:
                        throw new Exception("Invalid context type.");
                }
            }
            else
            {
                switch (contextType)
                {

                    case ContextType.SqlServer:
                        this.DataContext = ContextProvider.GetInstance<SqlServerContext>();
                        break;

                    default:
                        throw new Exception("Invalid context type.");
                }
            }
        }

        /// <summary>
        /// Dispose Context -- fecha a conexão do contexto
        /// </summary>
        public void Dispose()
        {
            if ((this.DataContext != null) && (this.IsDisposable))
            {
                // Dispose the data context
                this.DataContext.Dispose();
            }

            // For garbage collection optimization 
            GC.SuppressFinalize(this);
        }
    }
}
