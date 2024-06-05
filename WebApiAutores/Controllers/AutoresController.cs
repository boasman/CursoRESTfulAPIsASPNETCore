using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using WebApiAutores.Entidades;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/autores")]
    //[Route("api/[controller]")]  //placeholder
    public class AutoresController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet] //api/autores
        [HttpGet("listado")] //api/autores//listado
        [HttpGet("/listado")] //listado sobreescribiendo la ruta del controlador
        public async Task<ActionResult<List<Autor>>> Get()
        {
            return await context.Autores.Include(x=>x.Libros).ToListAsync();
        }

        [HttpGet("Primero")] //api/autores/primero contatena  la ruta del controladores con la del endpoint
        [HttpGet("PrimeroAutor")] //api/autores/primero contatena  la ruta del controladores con la del endpoint
        public async Task<ActionResult<Autor>> PrimerAutor()
        {

            return await context.Autores.FirstOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult> Post(Autor autor)
        {
            //No es correcto exponer las entidades al mundo externo 
            context.Autores.Add(autor);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Autor autor, int id)
        {
            if (autor.Id != id)
            {
                return BadRequest("El id del autor no coincide con el id de la url");
            }

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(autor);            
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {

            var existe = await context.Autores.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(new Autor { Id = id });
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
