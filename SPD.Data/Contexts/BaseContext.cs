using SPD.Data.EntityTypeConfigurations;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using SPD.Data.Utilities;
using SPD.Data.Initializers;

namespace SPD.Data.Contexts
{
    /// <summary>
    /// Classe responsável por ser a base do contexto do banco
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class BaseContext<TContext> : DbContext where TContext : BaseContext<TContext>
    {
        public dynamic Context { get; set; }

        /// <summary>
        /// The database name flag.
        /// </summary>
        internal ContextType ContextType { get; set; }

        /// <summary>
        /// The database schema.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// The object in charge to initialize the database.
        /// </summary>
        internal DatabaseInitializer<TContext> DatabaseInitializer { get; private set; }

        /// <summary>
        /// Base context constructor pointing to a connection string.
        /// </summary>
        public BaseContext(string connectionStringName)
            : base(connectionStringName)
        {
            // Set the default schema
            this.Schema = null;

            // Create the database initializer
            this.DatabaseInitializer = new DatabaseInitializer<TContext>((TContext)this);

            // Create the database with custom code
            Database.SetInitializer<TContext>(this.DatabaseInitializer);

            // Add the database Interceptor to see the generated sql
            DbInterception.Add(new DatabaseInterceptor());
        }

        /// <summary>
        /// Setup the default behavior for EntityFramework.
        /// </summary>
        /// <param name="modelBuilder">The builder for the model.</param>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Setup base conventions
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            // By default force property to key when its name contains "id" or "Id" or "iD" or "ID"
            modelBuilder.Properties()
                .Where(property => String.Equals(property.Name, String.Concat(property.ReflectedType.Name, "Id"), StringComparison.InvariantCultureIgnoreCase))
                .Configure(property => property.IsKey());
        }

        /// <summary>
        /// Setup the custom behavior for EntityFramework.
        /// </summary>
        /// <param name="modelBuilder">The builder for the model.</param>
        protected void OnModelCreated(DbModelBuilder modelBuilder)
        {
            // Get all types to configure at runtime
            var typesToConfigure = (
                from type in Assembly.GetExecutingAssembly().GetTypes()
                where type.IsClass && typeof(IConfiguration).IsAssignableFrom(type)
                select type
            ).ToList();

            // Add custom configurations to EntityFramework
            foreach (var type in typesToConfigure)
            {
                dynamic configuration = Activator.CreateInstance(type);

                modelBuilder.Configurations.Add(configuration);
            }
        }

        /// <summary>
        /// Setup the default behavior for save changes.
        /// </summary>
        /// <returns>The saved changes.</returns>
        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries().Where(entry => entry.Entity.GetType().GetProperty("DataCadastro") != null))
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Property("DataCadastro").CurrentValue = DateTime.Now;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Property("DataCadastro").IsModified = false;
                }
            }
            

            int changes = base.SaveChanges();

            return changes;
        }
        
        public void setProperty(object instance, string name, Type type, object value)
        {
            PropertyInfo prop = type.GetProperty(name);
            if (prop != null)
                prop.SetValue(instance, value, null);
        }
    }
}
