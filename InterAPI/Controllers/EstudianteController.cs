using InterAPI.DB;
using InterAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class EstudianteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EstudianteController(ApplicationDbContext context)
        {
            _context = context;          
        }

        [HttpPost]
        public async Task<IActionResult> CreateEstudiante([FromBody] Estudiante estudiante)
        {
            var validacionCorreo = await _context.Estudiante.FirstOrDefaultAsync(e => e.Correo == estudiante.Correo);
            var validacionDocumento = await _context.Estudiante.FirstOrDefaultAsync(e => e.NumeroDocumento == estudiante.NumeroDocumento);

            if (validacionCorreo == null & validacionDocumento == null)
            {
                estudiante.Creditos = 9;
                _context.Estudiante.Add(estudiante);

                var validacionLogin = await _context.Login.FirstOrDefaultAsync(e => e.correo == estudiante.Correo);

                var login = new Login();
                login.correo = estudiante.Correo;
                login.password = "Temp123*";

                if (validacionLogin == null)
                {
                    _context.Login.Add(login);
                }
                else
                {
                    return StatusCode(402, "El login ya se encuentra creado.");
                }

            }
            else
            {
                return StatusCode(404, "El estudiante ya se encuentra creado.");
            }

            await _context.SaveChangesAsync();
            return StatusCode(201,"Estudiante creado exitosamente.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEstudiante([FromBody] Estudiante estudiante)
        {
            var estudianteAntiguo = await _context.Estudiante.AsNoTracking().FirstOrDefaultAsync(e => e.Correo == estudiante.Correo);

            if (estudianteAntiguo == null)
            {
                return StatusCode(404, "No se encuentra usuario");
            }
            else
            {
                //Se quema el valor de los creditos para actualizaciones de estudiantes
                if (estudiante.Creditos == 99)
                {
                    estudiante.Creditos = estudianteAntiguo.Creditos;
                    estudiante.Id = estudianteAntiguo.Id;
                    _context.Estudiante.Update(estudiante);
                }
                else
                {
                    estudianteAntiguo.Creditos = estudiante.Creditos;
                    _context.Estudiante.Update(estudianteAntiguo);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(estudiante);
        }

        [HttpGet]
        public async Task<IActionResult> GetEstudianteByCorreo([FromQuery] string correo)
        {
            var estudiante = await _context.Estudiante.FirstOrDefaultAsync(e => e.Correo == correo);
            if (estudiante == null)
            {
                return NotFound();
            }
            return Ok(estudiante);
        }
    }
}