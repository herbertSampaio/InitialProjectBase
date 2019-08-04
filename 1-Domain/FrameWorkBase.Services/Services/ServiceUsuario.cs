using FrameWorkBase.Domain.Model;
using FrameWorkBase.Infra.Context;
using FrameWorkBase.Infra.Interfaces;
using FrameWorkBase.Infra.Validation.Interfaces;
using FrameWorkBase.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FrameWorkBase.Services.Services
{
    public class ServiceUsuario : ServiceBase<UsuarioDomain>, IServiceUsuario
    {
        private IUsuarioRepository UsuarioRepository { get; }
        private IUsuarioValidation Validation { get; }

        public ServiceUsuario(FrameWorkBaseContext dbContext, IUsuarioRepository usuarioRepository, IUsuarioValidation validation)
            : base(dbContext)
        {
            this.Validation = validation;
            this.UsuarioRepository = usuarioRepository;
        }

        public override List<string> ValidarEntity(UsuarioDomain entity)
        {
            if (entity.Id == 0)
                return this.Validation.IsAddValid(entity);
            else
                return this.Validation.IsUpdateValid(entity);
        }
    }
}
