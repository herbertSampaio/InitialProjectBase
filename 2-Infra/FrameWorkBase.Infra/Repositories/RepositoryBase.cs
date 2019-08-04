using FrameWorkBase.Infra.Context;
using FrameWorkBase.Infra.Interfaces;
using FrameWorkBase.Utilitario.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FrameWorkBase.Infra.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        private readonly FrameWorkBaseContext _dbContext;

        public RepositoryBase(FrameWorkBaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Any(Func<T, bool> predicate)
        {
            return _dbContext.Set<T>().Any(predicate);
        }

        public void Create(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public T Get(Func<T, bool> predicate)
        {
            return _dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefault(predicate);
        }

        public T Get(Func<T, bool> predicate, params Expression<Func<T, object>>[] includes)
        {
            var entity = _dbContext.Set<T>();

            if (includes == null)
            {
                return entity.AsNoTracking()
                    .FirstOrDefault(predicate);
            }
            else
            {
                foreach (var item in includes)
                {
                    entity.Include(item);
                }

                return entity
                .AsNoTracking()
                .FirstOrDefault(predicate);
            }
        }

        public IQueryable<T> GetAll()
        {

            return _dbContext.Set<T>().AsNoTracking();
        }

        public IQueryable<T> GetAll(Func<T, bool> predicate)
        {
            return _dbContext.Set<T>().AsNoTracking().Where(predicate).AsQueryable<T>();
        }

        public T GetById(int id)
        {
            return _dbContext.Set<T>()
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
        }

        public T GetById(int id, params Expression<Func<T, object>>[] includes)
        {
            var entity = _dbContext.Set<T>();

            if (includes == null)
            {
                return entity.AsNoTracking()
                    .FirstOrDefault(e => e.Id == id);
            }
            else
            {
                foreach (var item in includes)
                {
                    entity.Include(item);
                }

                return entity
                .AsNoTracking()
                .FirstOrDefault(e => e.Id == id);
            }
        }

        public void InsertList(List<T> list)
        {
            foreach (var item in list)
            {
                _dbContext.Set<T>().Add(item);
            }

            _dbContext.SaveChanges();
        }

        public void Update(int id, T entity)
        {
            var local = _dbContext.Set<T>().Local.FirstOrDefault(entry => entry.Id.Equals(id));

            if (local != null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            }

            _dbContext.Entry(entity).State = EntityState.Modified;

            _dbContext.SaveChanges();
        }
    }
}
