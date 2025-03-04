using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class FacturacionCLS
    {
        [Key]
        [Column("ID")] // indicar explícitamente a EF Core que la propiedad idFacturacion se mapea a la columna ID
        public int idFacturacion { get; set; }
        public int idPaciente { get; set; }
        public decimal monto { get; set; }
        public string metodoPago { get; set; }
        public DateTime fechaPago { get; set; }
        public int BHABILITADO { get; set; }
    }
}
