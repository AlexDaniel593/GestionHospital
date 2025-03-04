using CapaEntidad;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos
{
    public class CitaDAL
    {
        private readonly ApplicationDbContext _context;

        public CitaDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CitaCLS> ListarCita()
        {
            return _context.CitasResultado.ToList();
        }



        public void GuardarCita(CitaCLS cita)
        {
            var idParam = new SqlParameter("@idCita", cita.idCita);
            var idPacienteParam = new SqlParameter("@idPaciente", cita.idPaciente);
            var idMedicoParam = new SqlParameter("@idMedico", cita.idMedico);
            var fechaCitaParam = new SqlParameter("@fechaCita", cita.fechaHora);
            var estadoParam = new SqlParameter("@estado", cita.estado);
            _context.Database.ExecuteSqlRaw("EXEC uspGuardarCita @idCita, @idPaciente, @idMedico, @fechaCita, @estado",
                idParam, idPacienteParam, idMedicoParam, fechaCitaParam, estadoParam);

        }

        public CitaCLS? RecuperarCita(int idCita)
        {
            return _context.CITAS
                .FromSqlRaw("EXEC uspRecuperarCita @idCita", new SqlParameter("@idCita", idCita))
                .AsEnumerable()
                .FirstOrDefault();
        }

        public void EliminarCita(int idCita)
        {
            _context.Database.ExecuteSqlRaw("EXEC uspEliminarCita @idCita", new SqlParameter("@idCita", idCita));
        }

    }
}