using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CapaNegocio;
using CapaEntidad;
using System.Security.Claims;

namespace GestionHospital.Controllers
{
    [Authorize]
    public class TratamientoController : Controller
    {
        private readonly TratamientoBL _tratamientoBL;
       

        public TratamientoController(TratamientoBL tratamientoBL)
        {
            _tratamientoBL = tratamientoBL;
        }

        [Authorize(Roles = "Admin, Staff")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Staff")]
        public List<TratamientoCLS> ListarTratamiento()
        {
            return _tratamientoBL.ListarTratamiento();

        }

        [Authorize(Roles = "Admin, Doctor")]
        public TratamientoCLS? RecuperarTratamiento(int idTratamiento)
        {
            return _tratamientoBL.RecuperarTratamiento(idTratamiento);
        }

        [Authorize(Roles = "Admin")]
        public void EliminarTratamiento(int idTratamiento)
        {
            _tratamientoBL.EliminarTratamiento(idTratamiento);   
        }

        [Authorize(Roles = "Admin")]
        public void GuardarTratamiento(TratamientoCLS tratamiento)
        {
            _tratamientoBL.GuardarTratamiento(tratamiento);
        }


    }
}
