using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using CapaDatos;


namespace GestionHospital.Controllers
{
    public class PacienteController : Controller
    {
        private readonly PacienteBL _pacienteBL;

        public PacienteController(PacienteBL pacienteBL)
        {
            _pacienteBL = pacienteBL;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin,Receptionist,TreatmentSpecialist,Biller")]
        public List<PacienteCLS> ListarPaciente()
        {
            return _pacienteBL.ListarPaciente();
        }

        [Authorize(Roles = "Admin,Receptionist")]
        public void GuardarPaciente(PacienteCLS paciente)
        {
            _pacienteBL.GuardarPaciente(paciente);
        }

        [Authorize(Roles = "Admin,Receptionist,TreatmentSpecialist,Biller")]
        public PacienteCLS? RecuperarPaciente(int idPaciente)
        {
            return _pacienteBL.RecuperarPaciente(idPaciente);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        public void EliminarPaciente(int idPaciente)
        {
            _pacienteBL.EliminarPaciente(idPaciente);
        }
    }
}