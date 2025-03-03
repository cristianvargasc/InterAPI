using System.ComponentModel.DataAnnotations.Schema;

namespace InterAPI.Models
{
    public class Inscripcion
    {
        public int Id { get; set; }
        [ForeignKey("Materia")]
        public int IdMateria { get; set; }
        [ForeignKey("Estudiante")]
        public int IdEstudiante { get; set; }
        public DateTime fecha { get; set; }

        public Estudiante Estudiante { get; set; }
        public Materia Materia { get; set; }
    }
}
