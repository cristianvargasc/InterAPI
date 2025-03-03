using InterAPI.DB;
using InterAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLogin([FromBody] Login login)
        { 
            var validacionLogin = await _context.Login.FirstOrDefaultAsync(e => e.correo== login.correo);

            if (validacionLogin == null)
            {
                _context.Login.Add(login);
            }
            else
            {
                return StatusCode(404, "El login ya se encuentra creado.");
            }

            await _context.SaveChangesAsync();
            return StatusCode(201, "Login creado exitosamente.");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateLogin([FromBody] Login login)
        {
            var loginAntiguo = await _context.Login.AsNoTracking().FirstOrDefaultAsync(e => e.correo == login.correo);

            if (loginAntiguo == null)
            {
                return StatusCode(404, "No se encuentra usuario");
            }
            else
            {
                loginAntiguo.password = login.password;
                _context.Login.Update(loginAntiguo);
                await _context.SaveChangesAsync();
                return StatusCode(201, "Login actualizado.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLogin([FromQuery] Login login)
        {
            var loginAntiguo = await _context.Login.AsNoTracking().FirstOrDefaultAsync(e => e.correo == login.correo);
            
            if (loginAntiguo == null)
            {
                return NotFound();
            }
            else
            {                
                if (login.password == loginAntiguo.password)
                {
                    return Ok("Login exitoso.");
                }
                else
                {
                    return StatusCode(203, "Login rechazado.");
                }
            }
        }
    }
}
