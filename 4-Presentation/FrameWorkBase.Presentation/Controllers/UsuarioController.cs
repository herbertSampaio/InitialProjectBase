using AutoMapper;
using FrameWorkBase.Domain.Model;
using FrameWorkBase.Infra.Interfaces;
using FrameWorkBase.Infra.Validation.Interfaces;
using FrameWorkBase.Presentation.ViewModels;
using FrameWorkBase.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FrameWorkBase.Presentation.Controllers
{
    [Route("[controller]")]
    public class UsuarioController : ControllerApiBase<UsuarioDomain, UsuarioViewModel>
    {        
        public UsuarioController(IServiceUsuario repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }
    }
}