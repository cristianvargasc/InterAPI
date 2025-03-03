namespace InterAPI.Models
{
    public class Estudiante
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int TipoDocumento { get; set; }
        public long NumeroDocumento { get; set; }
        public string Correo { get; set; }
        public long Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Creditos { get; set; }
    }
}
