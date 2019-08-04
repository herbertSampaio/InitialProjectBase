using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.IdentityModel.Tokens;
using FrameWorkBase.Presentation.Security;
using FrameWorkBase.Presentation.ViewModels;
using FrameWorkBase.Services.Interfaces;
using AutoMapper;
using FrameWorkBase.Domain.Model;
using FrameWorkBase.Infra.Validation.Interfaces;

namespace FrameWorkBase.Presentation.Controllers
{
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IServiceUsuario _service;
        public AuthController(IServiceUsuario service)
        {
            this._service = service;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Post(
               [FromBody]UserAuth usuario,
               [FromServices]SigningConfigurations signingConfigurations,
               [FromServices]TokenConfiguration tokenConfigurations)
        {
            bool credenciaisValidas = false;
            if (usuario != null && !String.IsNullOrWhiteSpace(usuario.Login))
            {
                var usuarioBase = _service.Get(x=>x.Login == usuario.Login && x.Senha == usuario.Password);
                credenciaisValidas = usuarioBase != null;
            }

            if (credenciaisValidas)
            {
                ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(usuario.Login, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.UniqueName, usuario.Login)
                    }
                );

                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao +
                    TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = tokenConfigurations.Issuer,
                    Audience = tokenConfigurations.Audience,
                    SigningCredentials = signingConfigurations.SigningCredentials,
                    Subject = identity,
                    NotBefore = dataCriacao,
                    Expires = dataExpiracao
                });
                var token = handler.WriteToken(securityToken);

                return new
                {
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    accessToken = $"Bearer {token}",
                    message = "OK"
                };
            }
            else
            {
                return new
                {
                    authenticated = false,
                    message = "Falha ao autenticar"
                };
            }
        }
    }
}