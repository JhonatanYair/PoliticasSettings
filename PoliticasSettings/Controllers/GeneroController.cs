using Microsoft.AspNetCore.Mvc;
using PoliticasSettings.Models;
using PoliticasSettings.Repository;

namespace PoliticasSettings.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneroController : ControllerBase
    {
        IDbContextExtension extensionService;

        public GeneroController(IDbContextExtension service)
        {
            extensionService = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var generos = await extensionService.GetAllAsync<Genero>();
            return Ok(generos);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Genero genero)
        {
            await extensionService.AddAsync(genero);
            return Ok(); // Retorna el nuevo registro agregado
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Genero genero)
        {
            await extensionService.UpdateAsync(genero);
            return Ok(); // Retorna el nuevo registro agregado
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await extensionService.DeleteAsync<Genero>(id);
            return Ok();
        }
    }
}
