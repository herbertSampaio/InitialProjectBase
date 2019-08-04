using FrameWorkBase.Infra.Context;
using FrameWorkBase.Infra.Repositories;
using FrameWorkBase.Services.Interfaces;
using FrameWorkBase.Utilitario.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkBase.Services.Services
{
    public class ServiceBase<T> : RepositoryBase<T>, IServiceBase<T> where T : EntityBase
    {        
        public ServiceBase(FrameWorkBaseContext dbContext) : base(dbContext)
        {
        }

        public virtual List<string> ValidarEntity(T entity)
        {
            return new List<string>();
        }
    }
}
