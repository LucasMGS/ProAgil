using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.API.Dtos;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EventoController : ControllerBase
    {
        public IProAgilRepository _repo { get; set; }
        public IMapper _mapper { get; set; }

        public EventoController(IProAgilRepository repo,
                                IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsync(false);
                var results = _mapper.Map<EventoDto[]>(eventos);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int eventoId) 
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(eventoId, false);
                var results = _mapper.Map<EventoDto>(evento);

                return Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou :(");
            }
        }

        [HttpGet("/tema/{tema}")]
        public async Task<IActionResult> Get(string tema) 
        {
            try
            {
                var evento = await _repo.GetAllEventoAsyncByTema(tema,false);
                var results = _mapper.Map<EventoDto>(evento);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("upload")]
        public IActionResult Upload() 
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Resources", "Images");
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {   
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    var fullPath = Path.Combine(pathToSave, fileName.Replace("\""," ").Trim());

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }

                return Ok();
                
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            //return BadRequest("Erro ao tentar realizar upload");
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto eventoDto) 
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDto);
                _repo.Add(evento);
                
                var eventoDtoMapeado = _mapper.Map<EventoDto>(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{eventoDto.Id}", eventoDtoMapeado);
                }
                
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou :(");
            }

            return BadRequest();
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(int eventoId, EventoDto eventoDto) 
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(eventoId,false);

                if (evento == null)
                {
                    return NotFound();
                }

                var eventoDtoMapeado = _mapper.Map<EventoDto>(evento);

                _mapper.Map(eventoDto, evento);
                _repo.Update(evento);
                
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{eventoDto.Id}", eventoDtoMapeado);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete("{eventoId}")]
        public async Task<IActionResult> Delete(int eventoId) 
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(eventoId,false);

                if (evento == null)
                {
                    return NotFound();
                }

                _repo.Delete(evento);
                
                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou :(");
            }

            return BadRequest();
        }
    }
}