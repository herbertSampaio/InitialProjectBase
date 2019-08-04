using FrameWorkBase.Utilitario.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrameWorkBase.Infra.Interfaces
{
    public interface IRepositoryBase<T> where T:EntityBase
    {
        IQueryable<T> GetAll();

        IQueryable<T> GetAll(Func<T,bool> predicate);

        T GetById(int id);

        T Get(Func<T, bool> predicate);

        void Create(T entity);

        void Update(int id, T entity);

        void InsertList(List<T> list);

        void Delete(int id);

        void Delete(T entity);

        bool Any(Func<T, bool> predicate);
    }
}
