using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CapaEntidad
{
    public class CitaViewCLS
    {

        [Key]
        [Column("ID")]
        public int idCita { get; set; }
        public int idPaciente { get; set; }
        public string nombreCompletoPaciente { get; set; }
        public int idMedico { get; set; }
        public string nombreCompletoMedico { get; set; }
        
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime fechaHora { get; set; }
        
        public string estado { get; set; }
        public int BHABILITADO { get; set; }
    }



}