using SPD.Data.Utilities;
using SPD.Data.Interfaces.Contexts;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;

namespace SPD.Data.Contexts
{
    /// <summary>
    /// The Sql Server Compact context must have SSCERuntime_x64-PTB.exe or SSCERuntime_x86-PTB.exe installed. It works with Sql Server Compact v4.0.
    /// </summary>
    public sealed class SqlServerContext : BaseContext<SqlServerContext>, IDataContext
    {
        /// <summary>
        /// Return DbCOntext 
        /// </summary>
        public DbContext Entity
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Constructor of the class
        /// </summary>
        public SqlServerContext()
            : base("SqlServer")
        {
            // Set the context type
            this.ContextType = ContextType.SqlServer;
            
            // Set the schema
            this.Schema = ConfigurationManager
                .ConnectionStrings["SqlServer"]
                .ConnectionString
                .Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Where(token => token.Contains("User Id"))
                .First()
                .Split("=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Last();
        }

        /// <summary>
        /// Setup the SqlServer behavior for EntityFramework.
        /// </summary>
        /// <param name="modelBuilder">The builder for the model.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Setup the default behavior for EntityFramework
            base.OnModelCreating(modelBuilder);

            // Setup the string fields for SqlServer database type
            modelBuilder.Properties<string>()
                .Configure(property => property.HasColumnType("NVARCHAR"));

            // Setup the string fields default size
            modelBuilder.Properties<string>()
                .Configure(property => property.HasMaxLength(100));

            // Setup the custom behavior for EntityFramework
            base.OnModelCreated(modelBuilder);
        }
    }
}
