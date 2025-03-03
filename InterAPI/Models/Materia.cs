using System.ComponentModel.DataAnnotations.Schema;

namespace InterAPI.Models
{
    public class Materia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        [ForeignKey("Docente")]
        public int IdDocente { get; set; }
        public bool Estado { get; set; }
        public Docente Docente { get; set; }
    }
}
