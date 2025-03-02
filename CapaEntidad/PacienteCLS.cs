using System;
using System.ComponentModel.DataAnnotations;

namespace CapaEntidad
{
    public class PacienteCLS
    {
        [Key]
        public int idPaciente { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public DateTime? fechaNacimiento { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string direccion { get; set; }
        public int BHABILITADO { get; set; }
    }
}

