using CapaNegocio;
using CapaEntidad;
using Microsoft.AspNetCore.Mvc;

namespace GestionHospital.Controllers
{
    public class PacienteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public List<PacienteCLS> ListarPaciente()
        {
            PacienteBL pacienteBL = new PacienteBL();
            return pacienteBL.ListarPaciente();
        }
    }
}
