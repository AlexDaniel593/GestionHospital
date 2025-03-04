using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CitaBL
    {
        private readonly CitaDAL _citaDAL;

        public CitaBL(CitaDAL citaDAL)
        {
            _citaDAL = citaDAL;
        }

        public List<CitaCLS> ListarCita()
        {
            return _citaDAL.ListarCita();
        }

        public void GuardarCita(CitaCLS cita)
        {
            _citaDAL.GuardarCita(cita);
        }

        public CitaCLS? RecuperarCita(int idCita)
        {
            return _citaDAL.RecuperarCita(idCita);
        }

        public void EliminarCita(int idCita)
        {
            _citaDAL.EliminarCita(idCita);
        }


    }
}