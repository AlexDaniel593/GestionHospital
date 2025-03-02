using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;


namespace GestionHospital.Controllers
{
    public class PacienteController : Controller
    {
        private readonly PacienteBL _pacienteBL;

        public PacienteController(PacienteBL pacienteBL)
        {
            _pacienteBL = pacienteBL;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        public List<PacienteCLS> ListarPaciente()
        {
            return _pacienteBL.ListarPaciente();
        }
    }
}
