using FrameWorkBase.Utilitario.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkBase.Infra.Validation.Interfaces
{
    public interface IValidationBase<T> where T : EntityBase
    {
        bool IsEntityValid(int id);
    }
}
