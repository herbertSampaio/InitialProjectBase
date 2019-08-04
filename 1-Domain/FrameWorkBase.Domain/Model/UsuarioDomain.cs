using FrameWorkBase.Utilitario.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWorkBase.Domain.Model
{
    public class UsuarioDomain: EntityBase
    {
        public string Nome { get; private set; }
        public string Senha { get; private set; }
        public string Login { get; private set; }
    }
}
