using FrameWorkBase.Domain.Model;
using FrameWorkBase.Infra.Interfaces;
using FrameWorkBase.Infra.Validation.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FrameWorkBase.Infra.Validation.Validations
{
    public class UsuarioValidation : ValidationBase<UsuarioDomain>, IUsuarioValidation
    {
        private readonly IUsuarioRepository _repository;
        public UsuarioValidation(IUsuarioRepository repository) : base(repository)
        {
            _repository = repository;
        }

        public List<string> IsAddValid(UsuarioDomain entity)
        {
            var errors = new List<string>();
            var regexSenha = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,10})";

            if (string.IsNullOrEmpty(entity.Login))
                errors.Add("Informe um login válido");

            if (string.IsNullOrEmpty(entity.Senha))
                errors.Add("Informe uma senha válida");

            if (this._repository.Any(x => x.Login.ToLower() == entity.Login.ToLower() && x.Id != entity.Id))
                errors.Add("Login informado já está cadastrado");

            if (!Regex.IsMatch(entity.Senha, regexSenha))
                errors.Add(@"A senha informada não esta dentro do padrão, informe uma senha de 6 a 10 digitos, 
                            contendo uma letra maiuscula, uma letra minuscula e um caracter especial");

            return errors;
        }

        public List<string> IsUpdateValid(UsuarioDomain entity)
        {
            var errors = new List<string>();
            var regexSenha = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%]).{6,10})";

            if (string.IsNullOrEmpty(entity.Login))
                errors.Add("Informe um login válido");

            if (string.IsNullOrEmpty(entity.Senha))
                errors.Add("Informe uma senha válida");

            if (this._repository.Any(x => x.Login.ToLower() == entity.Login.ToLower() && x.Id != entity.Id))
                errors.Add("Login informado já está cadastrado");

            if (!Regex.IsMatch(entity.Senha, regexSenha))
                errors.Add(@"A senha informada não esta dentro do padrão, informe uma senha de 6 a 10 digitos, 
                            contendo uma letra maiuscula, uma letra minuscula e um caracter especial");

            return errors;
        }

        public bool ValidarLogin(string login)
        {
            return _repository.Any(x => x.Login.ToLower() == login.ToLower());
        }
    }
}
