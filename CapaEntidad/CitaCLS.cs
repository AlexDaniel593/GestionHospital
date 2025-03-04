using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public class CitaCLS
    {
        [Key]
        [Column("ID")] // indicar explícitamente a EF Core que la propiedad idCita se mapea a la columna ID
        public int idCita { get; set; }
        public int idPaciente { get; set; }
        public int idMedico { get; set; }
        public DateTime fechaHora { get; set; }
        public string estado { get; set; }

        public int BHABILITADO { get; set; }

    }
}
