using InterAPI.DB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MateriaController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MateriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMateria()
        {
            var materia = await _context.Materia
                .Include(e => e.Docente)
                .Where(e => e.Estado == true)
                .Select(e => new
                {
                    e.Id,
                    NombreMateria = e.Nombre,
                    IdDocente = e.IdDocente,
                    NombreDocente = e.Docente.Nombre
                })
                .ToListAsync();

            return Ok(materia);
        }
    }
}
