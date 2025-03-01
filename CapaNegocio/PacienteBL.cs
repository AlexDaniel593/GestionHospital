using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class PacienteBL
    {
        public List<PacienteCLS> ListarPaciente()
        {
            PacienteDAL pacienteDAL = new PacienteDAL();
            return pacienteDAL.ListarPaciente();
        }
    }
}
