using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class MedicoController : Controller
    {
        private readonly MedicoBL _medicoBL;

        public MedicoController(MedicoBL medicoBL)
        {
            _medicoBL = medicoBL;
        }

        [Authorize(Roles = "Admin, Staff")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        public List<MedicoCLS> ListarMedico()
        {
            return _medicoBL.ListarMedico();

        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GuardarMedico(MedicoCLS medico)
        {
            await _medicoBL.GuardarMedico(medico);
            return RedirectToAction("Index"); 

        }

        [Authorize(Roles = "Admin, Doctor")]
        public MedicoCLS? RecuperarMedico(int idMedico)
        {
            return _medicoBL.RecuperarMedico(idMedico);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EliminarMedico(int idMedico)
        {
            try
            {
                await _medicoBL.EliminarMedico(idMedico);
                return Ok("Médico deshabilitado con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al deshabilitar el médico: " + ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CrearCuentasParaMedicosExistentes()
        {
            try
            {
                await _medicoBL.CrearCuentasParaMedicosExistentes();
                return Ok("Cuentas de médicos creadas con éxito");
            }
            catch (Exception ex)
            {
                return BadRequest("Error al crear cuentas para médicos: " + ex.Message);
            }
        }

    }
}