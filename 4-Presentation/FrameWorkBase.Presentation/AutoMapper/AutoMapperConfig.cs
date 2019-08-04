using AutoMapper;
using FrameWorkBase.Domain.Model;
using FrameWorkBase.Presentation.ViewModels;
using FrameWorkBase.Utilitario.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrameWorkBase.Presentation.AutoMapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<EntityBase, ViewModelBase>().ReverseMap();
            CreateMap<UsuarioDomain, UsuarioViewModel>().ReverseMap();
        }
    }
}
