namespace ERP.Web.Domain.Dto
{
    public class EmpleadoDto
    {
        public int Id { get; set; }
        public int PersonaId { get; set; } 
        public string Nombre { get; set; } = null!;
        public DateTime? FechaDeNacimiento { get; set; }
        public decimal Sueldo { get; set; }

        public PersonaDto DatosPersonales { get; set; } = new(); 
    }
}

