using InterAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InterAPI.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Estudiante> Estudiante { get; set; }
        public DbSet<Materia> Materia { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Inscripcion> Inscripcion { get; set; }
    }
}
