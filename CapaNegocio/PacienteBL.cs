using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class PacienteBL
    {
        private readonly PacienteDAL _pacienteDAL;

        public PacienteBL(PacienteDAL pacienteDAL)
        {
            _pacienteDAL = pacienteDAL;
        }

        public List<PacienteCLS> ListarPaciente()
        {
            return _pacienteDAL.ListarPaciente();
        }
    }
}
