using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FrameWorkBase.Utilitario.Entities
{
    public class EntityBase
    {
        [Key]
        public int Id { get; private set; }
        public DateTime DataCriacao { get; private set; }
    }
}
