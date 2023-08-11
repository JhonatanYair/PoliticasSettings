using Microsoft.AspNetCore.Mvc;
using PoliticasSettings.Models;
using PoliticasSettings.Repository;
using PoliticasSettings.Datos;
using System.Net;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System.Linq.Expressions;

namespace PoliticasSettings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        IDbContextExtension extensionService;

        public PersonaController(IDbContextExtension service)
        {
            extensionService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var personas = await extensionService.GetAllAsync<Persona>();
            return Ok(personas);
        }

        [HttpGet("Get2")]
        public async Task<IActionResult> Get2()
        {
            var personas = await extensionService.GetAllAsyncInclude<Persona>();
            return Ok(personas);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Persona persona)
        {
            // Verificar si el ID de genero existe en la base de datos
            var existingGeneroTask = extensionService.GetByIdAsync<Genero>(persona.IdGenero);
            var existingGenero = await existingGeneroTask;

            if (existingGenero == null)
            {
                return BadRequest("El ID de género no existe en la base de datos.");
            }

            // Asignar el genero existente a la nueva persona
            persona.IdGeneroNavigation = existingGenero;

            await extensionService.AddAsync(persona);
            return Ok(); // Retorna el nuevo registro agregado

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Persona persona)
        {

            var existingGeneroTask = extensionService.GetByIdAsync<Genero>(persona.IdGenero);
            var existingGenero = await existingGeneroTask;

            if (existingGenero == null)
            {
                return BadRequest("El ID de género no existe en la base de datos.");
            }

            // Asignar el genero existente a la nueva persona
            persona.IdGeneroNavigation = existingGenero;

            await extensionService.UpdateAsync(persona);
            return Ok(); // Retorna el nuevo registro agregado
          
        }

        [HttpGet("GetFiltered2")]
        public async Task<IActionResult> GetFiltered([FromQuery] Persona filterModel)
        {
            Expression<Func<Persona, bool>> filterExpression = p => p.Nombre == filterModel.Nombre;
            var filteredPersonas = await extensionService.GetFilteredAsync(filterExpression);
            return Ok(filteredPersonas);
        }


        [HttpGet("GetFiltered")]
        public async Task<IActionResult> Get3()
        {

            //var dbContext = new DBPersonaContext();
            //var personaQuery = dbContext.Persona.AsQueryable();

            //var filterModel = new Persona
            //{
            //    Nombre = "Maria"
            //    // Add other filter properties if needed
            //};

            //var filteredPersona = personaQuery.ApplyFilter(filterModel).ToList();

            //return Ok(filteredPersona);

            //var filterModel = new Persona
            //{
            //    Nombre = "Maria"
            //    // Add other filter properties if needed
            //};

            //var filteredPersonas = await extensionService.GetFilteredAsync(filterModel);
            //return Ok(filteredPersonas);

            Expression<Func<Persona, bool>> filterExpression = p => p.Apellido == "Gomez";

            var filteredPersonas = await extensionService.GetFilteredAsync(filterExpression);
            return Ok(filteredPersonas);

        }

    }
}
