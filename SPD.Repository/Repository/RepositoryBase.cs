using SPD.Data.Contexts;
using SPD.Data.Utilities;
using SPD.Model.Util;
using SPD.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SPD.Repository.Repository
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {

        private dynamic _context;

        /// <summary>
        /// The context data from the front-end application.
        /// </summary>
        public dynamic Context
        {

            get { return _context; }
            set
            {
                _context = value;
                this.DomainContext.Context = value;
            }
        }

        /// <summary>
        /// The domain context.
        /// </summary>
        protected DomainContext DomainContext { get; }

        public Transactional Transactional { get; set; }

        public RepositoryBase()
        {
            // Define the domain context to be used
            //this.DomainContext = new DomainContext(ContextType.MySql, false);
            this.DomainContext = new DomainContext(ContextType.SqlServer, false);

            // Define the initial transactional value
            this.Transactional = null;
        }

        /// <summary>
        /// Restore Entity Framework State
        /// </summary>
        private void RestoreEntityFrameworkState()
        {

            foreach (var entry in this.DomainContext.DataContext.Entity.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Detached:
                        break;

                    case EntityState.Unchanged:
                        break;

                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;

                    case EntityState.Deleted:
                        entry.Reload();
                        break;

                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// Add entity type
        /// </summary>
        /// <param name="entity">Entity type</param>
        public void Add(TEntity entity)
        {
            if (this.Transactional != null)
            {
                if (this.Transactional.UseTransactionScope)
                {
                    using (TransactionScope transactionScope = this.Transactional)
                    {
                        this.DomainContext.DataContext.Entity.Set<TEntity>().Add(entity);

                        if (this.Transactional.AutoSave)
                        {
                            if (this.SaveChanges() > 0)
                            {
                                transactionScope.Complete();
                            }
                        }
                    }
                }
                else
                {
                    this.DomainContext.DataContext.Entity.Set<TEntity>().Add(entity);

                    if (this.Transactional.AutoSave)
                    {
                        this.SaveChanges();
                    }
                }
            }
            else
            {
                this.DomainContext.DataContext.Entity.Set<TEntity>().Add(entity);
            }
        }

        public void AddEntity(TEntity entity)
        {
            this.DomainContext.DataContext.Entity.Set<TEntity>().Add(entity);
        }

        public TEntity GetById(int id)
        {
            return this.DomainContext.DataContext.Entity.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            var entidade = this.DomainContext.DataContext.Entity.Set<TEntity>().ToList();

            if (entidade != null)
            {
                foreach (var item in entidade)
                {
                    this.DomainContext.DataContext.Entity.Entry<TEntity>(item).Reload();
                }
            }

            return entidade;
        }

        public IEnumerable<TEntity> GetAllAsNoTracking()
        {
            return this.DomainContext.DataContext.Entity.Set<TEntity>().AsNoTracking().ToList();
        }

        public IQueryable<TEntity> Query()
        {
            RefreshDatabaseCache();
            var query = this.DomainContext.DataContext.Entity.Set<TEntity>().AsQueryable();
            return query;
        }

        public IQueryable<TEntity> QueryAsNoTracking()
        {
            return this.DomainContext.DataContext.Entity.Set<TEntity>().AsNoTracking().AsQueryable();
        }

        public void Update(TEntity entity)
        {
            if (this.Transactional != null)
            {
                if (this.Transactional.UseTransactionScope)
                {
                    using (TransactionScope transactionScope = this.Transactional)
                    {
                        this.DomainContext.DataContext.Entity.Entry(entity).State = EntityState.Modified;

                        if (this.Transactional.AutoSave)
                        {
                            if (this.SaveChanges() > 0)
                            {
                                transactionScope.Complete();
                            }
                        }
                    }
                }
                else
                {
                    this.DomainContext.DataContext.Entity.Entry(entity).State = EntityState.Modified;

                    if (this.Transactional.AutoSave)
                    {
                        this.SaveChanges();
                    }
                }
            }
            else
            {
                this.DomainContext.DataContext.Entity.Entry(entity).State = EntityState.Modified;
            }
        }

        public void UpdateEntity(TEntity entity)
        {
            this.DomainContext.DataContext.Entity.Entry(entity).State = EntityState.Modified;
        }

        public void Save(TEntity entity, Expression<Func<TEntity, bool>> where)
        {
            foreach (var foundEntity in this.DomainContext.DataContext.Entity.Set<TEntity>().Where(where))
            {
                if (foundEntity != null)
                {
                    this.DomainContext.DataContext.Entity.Entry<TEntity>(foundEntity).State = EntityState.Detached;

                    this.DomainContext.DataContext.Entity.Set<TEntity>().Attach(entity);

                    this.Update(entity);
                }
                else
                {
                    this.Add(entity);
                }
            }
        }

        public void Remove(TEntity entity)
        {
            if (this.Transactional != null)
            {
                if (this.Transactional.UseTransactionScope)
                {
                    using (TransactionScope transactionScope = this.Transactional)
                    {
                        this.DomainContext.DataContext.Entity.Set<TEntity>().Remove(entity);

                        if (this.Transactional.AutoSave)
                        {
                            if (this.SaveChanges() > 0)
                            {
                                transactionScope.Complete();
                            }
                        }
                    }
                }
                else
                {
                    this.DomainContext.DataContext.Entity.Set<TEntity>().Remove(entity);

                    if (this.Transactional.AutoSave)
                    {
                        this.SaveChanges();
                    }
                }
            }
            else
            {
                this.DomainContext.DataContext.Entity.Set<TEntity>().Remove(entity);
                this.SaveChanges();
            }
        }

        public void DeleteEntity(TEntity entity)
        {
            this.DomainContext.DataContext.Entity.Set<TEntity>().Remove(entity);
        }

        public void SaveChange()
        {
            this.DomainContext.DataContext.Entity.SaveChanges();
        }

        public int SaveChanges(bool mustReload = true)
        {
            int result = -1;

            try
            {
                result = this.DomainContext.DataContext.Entity.SaveChanges();

                if (mustReload)
                {
                    foreach (var entry in this.DomainContext.DataContext.Entity.ChangeTracker.Entries())
                    {
                        entry.Reload();
                    }
                }

                return result;
            }
            catch (DbEntityValidationException dbEntityValidationException)
            {
                ContextDebugger.ShowInDebugConsole(dbEntityValidationException);

                throw dbEntityValidationException;
            }
            catch (Exception exception)
            {
                ContextDebugger.ShowInDebugConsole(exception);

                this.RestoreEntityFrameworkState();
            }

            return result;
        }


        public void RefreshDatabaseCache()
        {

        }

        public void Dispose()
        {
            if (this.DomainContext != null)
            {
                // Dispose the domain context
                this.DomainContext.Dispose();
            }

            // For garbage collection optimization 
            GC.SuppressFinalize(this);
        }

        public void RemoveRange(List<TEntity> listaEntity)
        {
            if (this.Transactional != null)
            {
                if (this.Transactional.UseTransactionScope)
                {
                    using (TransactionScope transactionScope = this.Transactional)
                    {
                        foreach (var entity in listaEntity)
                        {
                            this.DomainContext.DataContext.Entity.Set<TEntity>().Remove(entity);
                        }

                        if (this.Transactional.AutoSave)
                        {
                            if (this.SaveChanges() > 0)
                            {
                                transactionScope.Complete();
                            }
                        }
                    }
                }
                else
                {
                    foreach (var entity in listaEntity)
                    {
                        this.DomainContext.DataContext.Entity.Set<TEntity>().Remove(entity);
                    }


                    if (this.Transactional.AutoSave)
                    {
                        this.SaveChanges();
                    }
                }
            }
            else
            {
                foreach (var entity in listaEntity)
                {
                    this.DomainContext.DataContext.Entity.Set<TEntity>().Remove(entity);
                }
            }
        }

        public void AddRange(List<TEntity> listaEntity)
        {
            if (this.Transactional != null)
            {
                if (this.Transactional.UseTransactionScope)
                {
                    using (TransactionScope transactionScope = this.Transactional)
                    {
                        foreach (var entity in listaEntity)
                        {
                            this.DomainContext.DataContext.Entity.Set<TEntity>().Add(entity);
                        }

                        if (this.Transactional.AutoSave)
                        {
                            if (this.SaveChanges() > 0)
                            {
                                transactionScope.Complete();
                            }
                        }
                    }
                }
                else
                {
                    foreach (var entity in listaEntity)
                    {
                        this.DomainContext.DataContext.Entity.Set<TEntity>().Add(entity);
                    }

                    if (this.Transactional.AutoSave)
                    {
                        this.SaveChanges();
                    }
                }
            }
            else
            {
                foreach (var entity in listaEntity)
                {
                    this.DomainContext.DataContext.Entity.Set<TEntity>().Add(entity);
                }
            }

        }

        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["SQL"].ConnectionString;
        }

    }
}
