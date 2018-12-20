using SPD.Model.Util;
using SPD.Repository.Interface;
using SPD.Services.Interface;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SPD.Services.Services
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        private readonly IRepositoryBase<TEntity> _Repository;
        
        public dynamic Context { get; set; }
        
        public dynamic TransactionalMaps { get; set; }
        
        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            this.TransactionalMaps = new ExpandoObject();

            var transactionalMap = new Dictionary<string, Transactional>();

            foreach (var interfaceType in this.GetType().GetInterfaces())
            {
                foreach (var transactionalMapItem in Transactional.GetTransactionalMap(interfaceType))
                {
                    transactionalMap.Add(transactionalMapItem.Key, transactionalMapItem.Value);
                }
            }

            this.TransactionalMaps.FromService = transactionalMap;

            this._Repository = repository;
        }

        //public ServiceBase()
        //{
        //}

        public void Add(TEntity entity)
        {
            this._Repository.Transactional = Transactional.ExtractTransactional(this.TransactionalMaps);

            this._Repository.Add(entity);
        }
        
        public TEntity GetById(int id)
        {
            return this._Repository.GetById(id);
        }
        
        public IEnumerable<TEntity> GetAll()
        {
            return this._Repository.GetAll();
        }
        
        public IEnumerable<TEntity> GetAllAsNoTracking()
        {
            return this._Repository.GetAllAsNoTracking();
        }
        
        IQueryable<TEntity> IServiceBase<TEntity>.Query()
        {
            return this._Repository.Query();
        }

       
        IQueryable<TEntity> IServiceBase<TEntity>.QueryAsNoTracking()
        {
            return this._Repository.QueryAsNoTracking();
        }
        
        public void Update(TEntity entity)
        {
            this._Repository.Transactional = Transactional.ExtractTransactional(this.TransactionalMaps);

            this._Repository.Update(entity);
        }
        
        public void Remove(TEntity entity)
        {
            this._Repository.Transactional = Transactional.ExtractTransactional(this.TransactionalMaps);

            this._Repository.Remove(entity);
        }
        
        public void RemoveRange(List<TEntity> entities)
        {
            this._Repository.Transactional = Transactional.ExtractTransactional(this.TransactionalMaps);

            this._Repository.RemoveRange(entities);

        }
        
        public int SaveChanges()
        {
            return this._Repository.SaveChanges();
        }

        public int SaveChanges(TransactionScope transactionScope)
        {
            var changes = this._Repository.SaveChanges();

            transactionScope.Complete();

            return changes;
        }
        
        public void Dispose()
        {
            this._Repository.Dispose();
        }
    }
}

