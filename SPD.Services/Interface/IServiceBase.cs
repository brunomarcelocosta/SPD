using SPD.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SPD.Services.Interface
{
    public interface IServiceBase<TEntity> : IDisposable where TEntity : class
    {
        dynamic Context { get; set; }
        
        dynamic TransactionalMaps { get; set; }
        
        [Transactional(AutoSave = true, UseTransactionScope = false)]
        void Add(TEntity entity);
        
        TEntity GetById(int id);
        
        IEnumerable<TEntity> GetAll();
        
        IEnumerable<TEntity> GetAllAsNoTracking();
        
        IQueryable<TEntity> Query();
        
        IQueryable<TEntity> QueryAsNoTracking();
        
        [Transactional(AutoSave = true, UseTransactionScope = false)]
        void Update(TEntity entity);
        
        [Transactional(AutoSave = true, UseTransactionScope = false)]
        void Remove(TEntity entity);

        [Transactional(AutoSave = true, UseTransactionScope = false)]
        void RemoveRange(List<TEntity> entities);
        
        int SaveChanges();
        
        int SaveChanges(TransactionScope transactionScope);
    }
}

