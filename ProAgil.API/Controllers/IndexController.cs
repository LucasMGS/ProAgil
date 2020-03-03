using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.API.Data;
using ProAgil.API.Model;

namespace ProAgil.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IndexController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<Index> _logger;
        public DataContext Context;

        // public Index(ILogger<Index> logger)
        // {
        //     _logger = logger;
        // }

        
        public IndexController(DataContext context)
        {
            Context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var results = Context.Eventos.ToList();
                return Ok(results);
            }
            catch (Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Banco de dados falhou :(");
            }
        }

        [HttpGet("{id}")]
        public Evento Get(int id)
        {
            return Context.Eventos.FirstOrDefault(x => x.EventoId == id);
        }
    }
}
