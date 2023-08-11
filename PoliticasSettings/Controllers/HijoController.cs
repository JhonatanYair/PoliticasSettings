using Microsoft.AspNetCore.Mvc;
using PoliticasSettings.Models;
using PoliticasSettings.Repository;

namespace PoliticasSettings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HijoController : ControllerBase
    {

        IDbContextExtension extensionService;

        public HijoController(IDbContextExtension service)
        {
            extensionService = service;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await extensionService.DeleteAsync<Hijo>(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var hijos = await extensionService.GetAllAsync<Hijo>();
            return Ok(hijos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Hijo hijo)
        {
            // Verificar si el ID de persona existe en la base de datos
            var existingPersonaTask = extensionService.GetByIdAsync<Persona>(hijo.IdPersona);
            var existingPersona = await existingPersonaTask;

            var existingPadreTask = extensionService.GetByIdAsync<Padre>(hijo.IdPadre);
            var existingPadre = await existingPadreTask;

            if (existingPersona == null)
            {
                return BadRequest("El ID de persona no existe en la base de datos.");
            }else if (existingPadre == null)
            {
                return BadRequest("El ID de padre no existe en la base de datos.");
            }

            // Asignar el padre existente al nuevo hijo
            hijo.IdPadreNavigation = existingPadre;
            hijo.IdPersonaNavigation = existingPersona;

            await extensionService.AddAsync(hijo);
            return Ok(); // Retorna el nuevo registro agregado
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Hijo hijo)
        {

            // Verificar si el ID de persona existe en la base de datos
            var existingPersonaTask = extensionService.GetByIdAsync<Persona>(hijo.IdPersona);
            var existingPersona = await existingPersonaTask;

            var existingPadreTask = extensionService.GetByIdAsync<Padre>(hijo.IdPadre);
            var existingPadre = await existingPadreTask;

            if (existingPersona == null)
            {
                return BadRequest("El ID de persona no existe en la base de datos.");
            }
            else if (existingPadre == null)
            {
                return BadRequest("El ID de padre no existe en la base de datos.");
            }

            // Asignar el padre existente al nuevo hijo
            hijo.IdPadreNavigation = existingPadre;
            hijo.IdPersonaNavigation = existingPersona;

            await extensionService.UpdateAsync(hijo);
            return Ok(); // Retorna el nuevo registro agregado

        }

    }
}
