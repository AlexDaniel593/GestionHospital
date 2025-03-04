using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using Microsoft.AspNetCore.Authorization;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class CitaController : Controller
    {
        private readonly CitaBL _citaBL;
        public CitaController(CitaBL citaBL)
        {
            _citaBL = citaBL;
        }
        [Authorize(Roles = "Admin, Patient, Doctor, Staff")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        public List<CitaCLS> ListarCitas()
        {
            List<CitaCLS> citas = _citaBL.ListarCita();
            return citas;
        }

        [Authorize(Roles = "Admin, Staff, Doctor")]
        public void GuardarCita(CitaCLS cita)
        {
            _citaBL.GuardarCita(cita);
        }

        [Authorize(Roles = "Admin, Staff, Doctor")]
        public CitaCLS? RecuperarCita(int idCita)
        {
            return _citaBL.RecuperarCita(idCita);
        }

        [Authorize(Roles = "Admin, Staff, Doctor")]
        public void EliminarCita(int idCita)
        {
            _citaBL.EliminarCita(idCita);
        }

    }
}