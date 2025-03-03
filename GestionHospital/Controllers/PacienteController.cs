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

        [Authorize(Roles = "Admin,Receptionist,TreatmentSpecialist,Biller")]
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
        public async Task<IActionResult> GuardarPaciente(PacienteCLS paciente)
        {
            // Guardar el paciente en la base de datos y, si es nuevo, crear el usuario
            await _pacienteBL.GuardarPaciente(paciente);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin,Receptionist,TreatmentSpecialist,Biller")]
        public PacienteCLS? RecuperarPaciente(int idPaciente)
        {
            return _pacienteBL.RecuperarPaciente(idPaciente);
        }

        [Authorize(Roles = "Admin,Receptionist")]
        public async Task<IActionResult> EliminarPaciente(int idPaciente)
        {
            try
            {
                await _pacienteBL.EliminarPaciente(idPaciente);
                return Ok("Paciente eliminado con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al eliminar el paciente: " + ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearCuentasParaPacientesExistentes()
        {
            // Crear cuentas para pacientes existentes desde la capa de negocio
            await _pacienteBL.CrearCuentasParaPacientesExistentes();

            return RedirectToAction("Index", "Home");
        }
    }
}
