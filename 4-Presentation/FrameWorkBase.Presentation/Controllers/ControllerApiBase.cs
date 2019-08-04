using AutoMapper;
using FrameWorkBase.Infra.Validation.Interfaces;
using FrameWorkBase.Presentation.ViewModels;
using FrameWorkBase.Services.Interfaces;
using FrameWorkBase.Utilitario.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace FrameWorkBase.Presentation.Controllers
{
    public class ControllerApiBase<T, TViewModel> : Controller where T : EntityBase where TViewModel : ViewModelBase
    {
        private readonly IMapper _mapper;
        private readonly IServiceBase<T> _service;

        public ControllerApiBase(IServiceBase<T> repository, IMapper mapper)
        {
            this._mapper = mapper;
            this._service = repository;
        }

        [HttpGet]
        [Authorize("Bearer")]
        public virtual ActionResult GetAll()
        {
            try
            {
                var lista = this._service.GetAll();

                return Ok(_mapper.Map<IList<TViewModel>>(lista));
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpGet("{id}")]
        [Authorize("Bearer")]
        public virtual ActionResult Get(int id)
        {
            try
            {
                var entity = this._service.GetById(id);

                return Ok(_mapper.Map<TViewModel>(entity));
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpPost]
        [Authorize("Bearer")]
        public virtual ActionResult Post([FromBody]TViewModel entity)
        {
            try
            {
                var erros = new List<string>();
                var entityDomain = this._mapper.Map<T>(entity);

                erros = _service.ValidarEntity(entityDomain);

                if (erros.Count == 0)
                {
                    this._service.Create(entityDomain);

                    return Ok(this._mapper.Map<TViewModel>(entityDomain));
                }
                else
                {
                    foreach (var item in erros)
                    {
                        ModelState.AddModelError("Erro", item);
                    }

                    return BadRequest(ModelState);
                }
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize("Bearer")]
        public virtual ActionResult Update(int id, [FromBody]TViewModel entity)
        {
            try
            {                
                var erros = new List<string>();
                var entityDomain = this._mapper.Map<T>(entity);

                erros = _service.ValidarEntity(entityDomain);

                if (erros.Count == 0)
                {

                    this._service.Update(id, entityDomain);

                    return Ok(this._mapper.Map<TViewModel>(entityDomain));
                }
                else
                {
                    foreach (var item in erros)
                    {
                        ModelState.AddModelError("Erro", item);
                    }

                    return BadRequest(ModelState);
                }

            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("Bearer")]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                //if (this._validation.IsEntityValid(id))
                //{
                this._service.Delete(id);

                return Ok("Deletado com sucesso!");
                //}
                //else
                //    return BadRequest("Informe um Id válido!");
            }
            catch (Exception exc)
            {
                return BadRequest(exc.Message);
            }
        }
    }
}
