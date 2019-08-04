using FrameWorkBase.Domain.Model;
using FrameWorkBase.Infra.Context;
using FrameWorkBase.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkBase.Infra.Repositories
{
    public class UsuarioRepository : RepositoryBase<UsuarioDomain>, IUsuarioRepository
    {
        private readonly FrameWorkBaseContext _dbContext;
        public UsuarioRepository(FrameWorkBaseContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
