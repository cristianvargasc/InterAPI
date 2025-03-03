using System.ComponentModel.DataAnnotations.Schema;

namespace InterAPI.Models
{
    public class MateriaRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }        
        public int IdDocente { get; set; }
    }
}
