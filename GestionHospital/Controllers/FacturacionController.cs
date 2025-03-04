using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using System.Security.Claims;


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
        public List<FacturacionCLS> ListarFacturacion()
        {
            return _facturacionBL.ListarFacturacion();
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
    }
}
