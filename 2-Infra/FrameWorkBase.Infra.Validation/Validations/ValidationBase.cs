using FrameWorkBase.Infra.Interfaces;
using FrameWorkBase.Infra.Validation.Interfaces;
using FrameWorkBase.Utilitario.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkBase.Infra.Validation.Validations
{
    public class ValidationBase<T> : IValidationBase<T> where T : EntityBase
    {
        private readonly IRepositoryBase<T> _repository;

        public ValidationBase(IRepositoryBase<T> repository)
        {
            this._repository = repository;
        }

        public bool IsEntityValid(int id)
        {
            return this._repository.Any(x => x.Id == id);
        }
    }
}
