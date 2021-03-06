﻿using SPD.Model.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPD.Repository.Interface
{
    public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
    {
        dynamic Context { get; set; }

        Transactional Transactional { get; set; }

        void Add(TEntity entity);

        void AddEntity(TEntity entity);

        TEntity GetById(int id);

        IEnumerable<TEntity> GetAll();

        IEnumerable<TEntity> GetAllAsNoTracking();

        IQueryable<TEntity> Query();

        IQueryable<TEntity> QueryAsNoTracking();

        void Update(TEntity entity);

        void Remove(TEntity entity);

        void DeleteEntity(TEntity entity);

        void SaveChange();

        int SaveChanges(bool mustReload = true);

        void RemoveEntityRange(List<TEntity> listaEntity);

        void RemoveRange(List<TEntity> listaEntity);

        void AddEntityRange(List<TEntity> listaEntity);

        void AddRange(List<TEntity> listaEntity);

        string GetConnectionString();

    }
}
