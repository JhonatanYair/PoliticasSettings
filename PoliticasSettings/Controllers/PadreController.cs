using Microsoft.AspNetCore.Mvc;
using PoliticasSettings.Models;
using PoliticasSettings.Repository;

namespace PoliticasSettings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PadreController : ControllerBase
    {
        IDbContextExtension extensionService;

        public PadreController(IDbContextExtension service)
        {
            extensionService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var padres = await extensionService.GetAllAsync<Padre>();
            return Ok(padres);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Padre padre)
        {
            // Verificar si el ID de genero existe en la base de datos
            var existingPersonaTask = extensionService.GetByIdAsync<Persona>(padre.IdPersona);
            var existingPersona = await existingPersonaTask;

            if (existingPersona == null)
            {
                return BadRequest("El ID de persona no existe en la base de datos.");
            }

            // Asignar el genero existente a la nueva persona
            padre.IdPersonaNavigation = existingPersona;

            await extensionService.AddAsync(padre);
            return Ok(); // Retorna el nuevo registro agregado
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Padre padre)
        {

            var existingPersonaTask = extensionService.GetByIdAsync<Persona>(padre.IdPersona);
            var existingPersona = await existingPersonaTask;

            if (existingPersona == null)
            {
                return BadRequest("El ID de género no existe en la base de datos.");
            }

            // Asignar el genero existente a la nueva persona
            padre.IdPersonaNavigation = existingPersona;

            await extensionService.UpdateAsync(padre);
            return Ok(); // Retorna el nuevo registro agregado

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await extensionService.DeleteAsync<Persona>(id);
            return Ok();
        }

    }
}
