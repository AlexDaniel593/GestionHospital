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

        public void GuardarPaciente(PacienteCLS paciente)
        {
            _pacienteDAL.GuardarPaciente(paciente);
        }

        public PacienteCLS? RecuperarPaciente(int idPaciente)
        {
            return _pacienteDAL.RecuperarPaciente(idPaciente);
        }

        public void EliminarPaciente(int idPaciente)
        {
            _pacienteDAL.EliminarPaciente(idPaciente);
        }


    }
}
