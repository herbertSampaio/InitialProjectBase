using FrameWorkBase.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkBase.Infra.Validation.Interfaces
{
    public interface IUsuarioValidation:IValidationBase<UsuarioDomain>
    {
        bool ValidarLogin(string login);
        List<string> IsAddValid(UsuarioDomain entity);
        List<string> IsUpdateValid(UsuarioDomain entity);
    }
}
