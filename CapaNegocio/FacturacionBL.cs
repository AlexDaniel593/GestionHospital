using CapaDatos;
using CapaEntidad;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class FacturacionBL
    {
        private readonly FacturacionDAL _facturacionDAL;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public FacturacionBL(FacturacionDAL facturacionDAL, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AccountBL accountBL, ApplicationDbContext context)
        {
            _facturacionDAL = facturacionDAL;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public List<FacturacionCLS> ListarFacturacion()
        {
            return _facturacionDAL.ListarFacturacion();
        }

        public FacturacionCLS? RecuperarFacturacion(int idFacturacion)
        {
            return _facturacionDAL.RecuperarFacturacion(idFacturacion);
        }

        public void EliminarFacturacion(int idFacturacion)
        {
            _facturacionDAL.EliminarFacturacion(idFacturacion);

        }
    }
}
