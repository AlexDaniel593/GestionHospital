using CapaEntidad;
using Microsoft.EntityFrameworkCore;

namespace CapaDatos
{
    public class PacienteDAL
    {
        private readonly ApplicationDbContext _context;

        public PacienteDAL(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<PacienteCLS> ListarPaciente()
        {
            return _context.PACIENTES.FromSqlRaw("EXEC uspListarPacientes").ToList();
        }
    }
}
