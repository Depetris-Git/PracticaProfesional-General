﻿using AutoMapper;
using WebITSC.Admin.Server.Repositorio;

using WebITSC.DB.Data;
using WebITSC.DB.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using WebITSC.Shared.General.DTO;

namespace WebITSC.Server.Controllers.General
{
    [ApiController]
    [Route("api/InscripcionCarrera")]
    public class InscripcionCarreraController : ControllerBase
    {
        private readonly IInscripcionCarreraRepositorio eRepositorio;
        private readonly IMapper mapper;

        public InscripcionCarreraController(IInscripcionCarreraRepositorio eRepositorio,
                                            IMapper mapper)
        {

            this.eRepositorio = eRepositorio;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<List<InscripcionCarrera>>> GetAll()
        {
            var inscrpicionCarreras = await eRepositorio.FullGetAll();

            return Ok(inscrpicionCarreras);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<InscripcionCarrera>> GetById(int id)
        {
            var inscrpicionCarrera = await eRepositorio.FullGetById(id);
            if (inscrpicionCarrera == null) return NotFound();

            return Ok(inscrpicionCarrera);
        }
        #region Peticiones Get

        //[HttpGet]
        //public async Task<ActionResult<List<InscripcionCarrera>>> Get()
        //{
        //    return await repositorio.Select();
        //}

        //[HttpGet("{id:int}")]
        //public async Task<ActionResult<InscripcionCarrera>> Get(int id)
        //{
        //    InscripcionCarrera? sel = await repositorio.SelectById(id);
        //    if (sel == null)
        //    {
        //        return NotFound();
        //    }
        //    return sel;
        //}

        //[HttpGet("existe/{id:int}")]
        //public async Task<ActionResult<bool>> Existe(int id)
        //{
        //    var existe = await repositorio.Existe(id);
        //    return existe;

        //}

        #endregion

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearInscripcionCarreraDTO entidadDTO)
        {
            try
            {
                InscripcionCarrera entidad = mapper.Map<InscripcionCarrera>(entidadDTO);

                return await eRepositorio.Insert(entidad);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, [FromBody] InscripcionCarrera entidad)
        {
            if (id != entidad.Id)
            {
                return BadRequest("Datos incorrectos");
            }
            var sel = await eRepositorio.SelectById(id);
            //sel = Seleccion

            if (sel == null)
            {
                return NotFound("No existe el tipo de documento buscado.");
            }


            sel = mapper.Map<InscripcionCarrera>(entidad);

            try
            {
                await eRepositorio.Update(id, sel);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await eRepositorio.Existe(id);
            if (!existe)
            {
                return NotFound($"El InscripcionCarrera {id} no existe");
            }
            InscripcionCarrera EntidadABorrar = new InscripcionCarrera();
            EntidadABorrar.Id = id;

            if (await eRepositorio.Delete(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }

        }



    }

}



