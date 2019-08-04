using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using AutoMapper;
using Newtonsoft.Json;
using System;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using FrameWorkBase.Infra.Context;
using FrameWorkBase.Infra.Interfaces;
using FrameWorkBase.Infra.Repositories;
using FrameWorkBase.Infra.Validation.Interfaces;
using FrameWorkBase.Infra.Validation.Validations;
using FrameWorkBase.Presentation.Security;
using FrameWorkBase.Services.Interfaces;
using FrameWorkBase.Services.Services;

namespace FrameWorkBase.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment CurrentEnvironment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment currentEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = currentEnvironment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc()
                .AddJsonOptions(json =>
                {
                    json.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    json.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });


            services
                .AddEntityFrameworkSqlServer()
                .AddDbContext<FrameWorkBaseContext>(opt => opt
                                                           .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("FrameWorkBase.Presentation"))
                                                           .EnableSensitiveDataLogging(true), ServiceLifetime.Scoped);
                    
            

            services.AddAutoMapper();

            this._ConfigureInjectionDependecy(services);
            this._ConfigureAuth(services);
            this._ConfigureSwagger(services);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app, IHostingEnvironment env,
            IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();


            if (!CurrentEnvironment.IsEnvironment("Test"))
            {
                //env.ConfigureNLog("nlog.config");
                //loggerFactory.AddNLog();
                
                serviceProvider.GetService<FrameWorkBaseContext>().Database.Migrate();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "FrameWorkBase API");
                });

                // app.UseResponseCompression();

            }

            app.UseMvc();
        }


        private void _ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "FrameWorkBase",
                    Description = "API FrameWorkBase",
                    TermsOfService = "None",
                    
                });

                s.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header use o schema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                s.AddSecurityRequirement(security);

            });
        }

        private void _ConfigureInjectionDependecy(IServiceCollection services)
        {
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));

            services
            .AddScoped<IUsuarioRepository, UsuarioRepository>()
            .AddScoped<IUsuarioValidation, UsuarioValidation>()
            .AddScoped<IServiceUsuario, ServiceUsuario>()

            ;
        }

        private void _ConfigureAuth(IServiceCollection services)
        {
            var signingConfigurations = new SigningConfigurations();
            services.AddSingleton(signingConfigurations);

            var tokenConfigurations = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(Configuration
                            .GetSection("TokenConfigurations"))
                            .Configure(tokenConfigurations);

            services.AddSingleton(tokenConfigurations);

            services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(bearerOptions =>
            {
                var paramsValidation = bearerOptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = signingConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfigurations.Audience;
                paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

                // Valida a assinatura de um token recebido
                paramsValidation.ValidateIssuerSigningKey = true;

                // Verifica se um token recebido ainda é válido
                paramsValidation.ValidateLifetime = true;

                // Tempo de tolerância para a expiração de um token (utilizado
                // caso haja problemas de sincronismo de horário entre diferentes
                // computadores envolvidos no processo de comunicação)
                paramsValidation.ClockSkew = TimeSpan.Zero;
            });

            // Ativa o uso do token como forma de autorizar o acesso
            // a recursos deste projeto
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }
    }
}
