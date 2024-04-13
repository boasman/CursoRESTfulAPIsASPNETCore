using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using WebApiAutores.Entidades;
using WebApiAutores.Migrations;

namespace WebApiAutores.Controllers
{

    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Libro>>> Get()
        {
            return await context.Libros.ToListAsync();
            
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Libro>> Get(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);
            if (!existe)
            {
                return NotFound();
            }
            return await context.Libros.FirstOrDefaultAsync(x=>x.Id == id);
        }

        [HttpPost]

        public async Task<ActionResult> Post(Libro libro)
        {
            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{int:id}")]
        public async Task<ActionResult> Put(Libro libro, int id)
        {
            if(libro.Id != id)
            {
                return BadRequest("El id del libro no coincide con el id de la url");
            }
            var existe = await context.Libros.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Update(libro);
            await context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var existe = await context.Libros.AnyAsync(x => x.Id == id);

            if (!existe)
            {
                return NotFound();
            }

            context.Remove(id);
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}
