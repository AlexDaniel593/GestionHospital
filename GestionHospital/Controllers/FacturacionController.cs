using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;


namespace GestionHospital.Controllers
{
    [Authorize]
    public class FacturacionController : Controller
    {
        private readonly FacturacionBL _facturacionBL;


        public FacturacionController(FacturacionBL facturacionBL)
        {
            _facturacionBL = facturacionBL;
        }

        [Authorize(Roles = "Admin, Staff")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        public List<FacturacionViewCLS> ListarFacturacion()
        {
            List<FacturacionViewCLS> facturacion = _facturacionBL.ListarFacturacion();
            return facturacion;
        }

        [Authorize(Roles = "Admin, Doctor")]
        public FacturacionCLS? RecuperarFacturacion(int idFacturacion)
        {
            return _facturacionBL.RecuperarFacturacion(idFacturacion);
        }

        [Authorize(Roles = "Admin")]
        public void EliminarFacturacion(int idFacturacion)
        {
            _facturacionBL.EliminarFacturacion(idFacturacion);
        }

        [Authorize(Roles = "Admin")]
        public void GuardarFacturacion(FacturacionCLS facturacion)
        {
            _facturacionBL.GuardarFacturacion(facturacion);
        }
    }
}
