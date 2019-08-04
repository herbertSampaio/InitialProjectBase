using FrameWorkBase.Infra.Interfaces;
using FrameWorkBase.Utilitario.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkBase.Services.Interfaces
{
    public interface IServiceBase<T> : IRepositoryBase<T> where T : EntityBase
    {
        List<string> ValidarEntity(T entity);
    }
}
