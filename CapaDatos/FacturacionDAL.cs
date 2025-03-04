using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos
{
    public class FacturacionDAL
    {
        private readonly ApplicationDbContext _context;

        public FacturacionDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<FacturacionCLS> ListarFacturacion()
        {
            return _context.FACTURACION.FromSqlRaw("EXEC uspListarFacturacion").ToList();
        }

        public void GuardarFacturacion(FacturacionCLS facturacion)
        {
            var idFacturacionParam = new SqlParameter("@idFacturacion", facturacion.idFacturacion);
            var idPacienteParam = new SqlParameter("@idPaciente", facturacion.idPaciente);
            var montoParam = new SqlParameter("@monto", facturacion.monto);
            var metodoPagoParam = new SqlParameter("@metodoPago", facturacion.metodoPago);
            var fechaPagoParam = new SqlParameter("@fechaPago", facturacion.fechaPago);

            _context.Database.ExecuteSqlRaw("EXEC uspGuardarFacturacion @idFacturacion, @idPaciente, @monto, @metodoPago, @fechaPago, @BHABILITADO",
                idFacturacionParam, idPacienteParam, montoParam, metodoPagoParam, fechaPagoParam);
        }

        public FacturacionCLS? RecuperarFacturacion(int idFacturacion)
        {
            return _context.FACTURACION
                .FromSqlRaw("EXEC uspRecuperarFacturacion @idFacturacion", new SqlParameter("@idFacturacion", idFacturacion))
                .AsEnumerable()
                .FirstOrDefault();
        }

        public void EliminarFacturacion(int idFacturacion)
        {
            _context.Database.ExecuteSqlRaw("EXEC uspEliminarFacturacion @idFacturacion", new SqlParameter("@idFacturacion", idFacturacion));
        }
    }
}
