using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EventoController : ControllerBase
    {
        public IProAgilRepository _repo { get; set; }
        public EventoController(IProAgilRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            try
            {
                var results = await _repo.GetAllEventoAsync(false);
                return Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou :(");
            }
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int eventoId) 
        {
            try
            {
                var results = await _repo.GetEventoAsyncById(eventoId, false);
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
                var results = await _repo.GetAllEventoAsyncByTema(tema,false);
                return Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou :(");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento evento) 
        {
            try
            {
                _repo.Add(evento);
                
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                }
                
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou :(");
            }

            return BadRequest();
        }

        [HttpPut("{eventoId}")]
        public async Task<IActionResult> Put(Evento evento) 
        {
            try
            {
                //var evento = await _repo.GetEventoAsyncById(eventoId,false);

                if (evento == null)
                {
                    return NotFound();
                }

                _repo.Update(evento);
                
                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                }
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou :(");
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