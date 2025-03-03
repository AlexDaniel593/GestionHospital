using CapaDatos;
using CapaEntidad;
using Microsoft.AspNetCore.Identity;

namespace CapaNegocio
{
    public class PacienteBL
    {
        private readonly PacienteDAL _pacienteDAL;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PacienteBL(PacienteDAL pacienteDAL, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, AccountBL accountBL)
        {
            _pacienteDAL = pacienteDAL;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public List<PacienteCLS> ListarPaciente()
        {
            return _pacienteDAL.ListarPaciente();
        }
        public async Task GuardarPaciente(PacienteCLS paciente)
        {
            _pacienteDAL.GuardarPaciente(paciente);

            // Si el paciente es nuevo (idPaciente == 0), crear el usuario en AspNetUsers
            if (paciente.idPaciente == 0)
            {
                var contrasena = AccountBL.GenerarContrasena(paciente.nombre, paciente.apellido, paciente.fechaNacimiento);

                // Await the asynchronous method call
                await CrearUsuarioPaciente(paciente.email, contrasena);
            }
        }

        public PacienteCLS? RecuperarPaciente(int idPaciente)
        {
            return _pacienteDAL.RecuperarPaciente(idPaciente);
        }

        public async Task EliminarPaciente(int idPaciente)
        {

            _pacienteDAL.EliminarPaciente(idPaciente);  // Actualiza el estado de BHABILITADO a 0

            // deshabilitar la cuenta del usuario correspondiente
            var paciente = _pacienteDAL.RecuperarPaciente(idPaciente); 
            if (paciente != null && paciente.email != null)
            {
                // Verifica si el usuario existe en AspNetUsers
                var user = await _userManager.FindByEmailAsync(paciente.email);
                if (user != null)
                {
                    // Bloquea la cuenta para que no pueda hacer login
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.UtcNow.AddYears(100);  // El lockout por 100 años

                    // Guarda los cambios
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception("Error al deshabilitar la cuenta del paciente: " + errors);
                    }
                }
            }
        }


        public async Task CrearUsuarioPaciente(string email, string password)
        {
            try
            {
                // Verificar si el usuario ya existe
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    user = new IdentityUser
                    {
                        UserName = email,
                        Email = email
                    };

                    // Crear el usuario con la contraseña predeterminada
                    var result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        // Asignar el rol "Patient" al usuario
                        await _userManager.AddToRoleAsync(user, "Patient");
                    }
                    else
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        throw new Exception("Error al crear el usuario: " + errors);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error en CrearUsuarioPaciente: " + ex.Message);
            }
        }

        public async Task CrearCuentasParaPacientesExistentes()
        {
            // Verificar si el rol "Patient" existe, si no, crearlo
            if (!await _roleManager.RoleExistsAsync("Patient"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Patient"));
            }

            // Obtener todos los pacientes habilitados (BHABILITADO = 1)
            var pacientes = _pacienteDAL.ListarPaciente();  // Podrías tener una consulta en DAL que filtre esto.

            foreach (var paciente in pacientes)
            {
                if (paciente.BHABILITADO == 1)
                {
                    // Generar la contraseña utilizando la lógica de AccountBL
                    var contrasena = AccountBL.GenerarContrasena(paciente.nombre, paciente.apellido, paciente.fechaNacimiento);

                    // Crear el usuario en AspNetUsers
                    await CrearUsuarioPaciente(paciente.email, contrasena);
                }
            }
        }
    }
}
