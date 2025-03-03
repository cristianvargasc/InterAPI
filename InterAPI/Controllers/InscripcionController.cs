using InterAPI.DB;
using InterAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class InscripcionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InscripcionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInscripcion([FromBody] List<MateriaRequest> materias, string correo)
        {
            var estudiante = await _context.Estudiante.AsNoTracking().FirstOrDefaultAsync(e => e.Correo == correo);

            if (estudiante == null)
            {
                return StatusCode(401, "No se encuentra el estudiante solicitado.");
            }
            else
            {
                if (estudiante.Creditos >= 3)
                {
                    if (estudiante.Creditos >= materias.Count * 3)
                    {
                        foreach (var materia in materias)
                        {
                            var validacionInscripcion = await _context.Inscripcion.FirstOrDefaultAsync(e => e.IdEstudiante == estudiante.Id
                                                                                                        && e.IdMateria == materia.Id);
                            if (validacionInscripcion == null)
                            {
                                //Validar la otra materia del docente
                                var materiaDocente = await _context.Materia.FirstOrDefaultAsync(e => e.IdDocente == materia.IdDocente
                                                                                                    && e.Id != materia.Id);

                                //Validar que no este inscrita otra materia del profesor
                                var validacionInscripcionOtra = await _context.Inscripcion.FirstOrDefaultAsync(e => e.IdEstudiante == estudiante.Id
                                                                                                        && e.IdMateria == materiaDocente.Id);

                                if (validacionInscripcionOtra == null)
                                {
                                    var inscripcion = new Inscripcion();
                                    inscripcion.IdMateria = materia.Id;
                                    inscripcion.IdEstudiante = estudiante.Id;
                                    inscripcion.fecha = DateTime.Now;
                                    _context.Inscripcion.Add(inscripcion);
                                    estudiante.Creditos = estudiante.Creditos - 3;
                                    _context.Estudiante.Update(estudiante);
                                    await _context.SaveChangesAsync();
                                }
                                else//Cuando tiene otra materia inscrita con el mismo profesor
                                {
                                    return StatusCode(403, "La materia " + materia.Nombre + ", no se permite inscribir, ya se cuenta con una materia con este docente.");
                                }
                            }
                            else//Cuando ya inscribió esa materia
                            {
                                return StatusCode(403, "La materia " + materia.Nombre + ", ya se encuentra inscrita.");
                            }
                        }
                        await _context.SaveChangesAsync();
                        return StatusCode(200, "Inscripción exitosa de materias.");
                    }
                    else //Cuando escogió más materias de las posibles en creditos
                    {
                        return StatusCode(403, "No se puede agregar más materias de las permitidas.");
                    }
                }
                else
                {
                    return StatusCode(403, "No se cuentan con creditos disponibles para agregar materias.");
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetInscripcion([FromQuery] string correo)
        {
            var estudiante = await _context.Estudiante.AsNoTracking().FirstOrDefaultAsync(e => e.Correo == correo);
            if (estudiante == null)
            {
                return NotFound("No hay materias inscritas.");
            }
            else
            {
                var inscripcion = await _context.Inscripcion
                    .Include(e => e.Materia.Docente)
                    .Where(e => e.IdEstudiante == estudiante.Id)
                    .Select(e => new
                    {
                        idMateria = e.IdMateria,
                        fechaInscripcion = e.fecha,
                        nombreMateria = e.Materia.Nombre,
                        nombreDocente = e.Materia.Docente.Nombre
                    })
                    .ToListAsync();
                return Ok(inscripcion);
            }            
        }

        [HttpGet("materia")]
        public async Task<IActionResult> GetInscripcionByMateria([FromQuery] MateriaRequest materia)
        {
            var inscripcion = await _context.Inscripcion
                .Include(e => e.Estudiante)
                .Where(e => e.IdMateria == materia.Id)
                .Select(e => new
                {
                    EstudianteNombre = e.Estudiante.Nombre,
                    FechaInscripcion = e.fecha
                })
                .ToListAsync();

            return Ok(inscripcion);
        }
    }
}
