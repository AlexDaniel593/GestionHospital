# GestionHospital
Programacion Web Proyecto Parcial 3
Integrantes:
- Daniel Guaman
- Kevin Amaguana

> [!IMPORTANT]
> # Cuentas de Prueba
>
> A continuación, encontrarás las credenciales de prueba para los distintos roles del sistema.  
> Úsalas para acceder y probar las funcionalidades según el rol asignado.

### 🔑 Credenciales de Prueba  

| Rol                  | Email                           | Contraseña              |
|----------------------|--------------------------------|-------------------------|
| **Administrador**    | `admin@admin.com`             | `Admin1234,`             |
| **Doctor**          | `doctor@test.com`             | `Doctor1234!`           |
| **Recepcionista**   | `receptionist@test.com`       | `Receptionist1234!`     |
| **Especialista en Tratamientos** | `treatmentspecialist@test.com` | `TreatmentSpecialist1234!` |
| **Facturador**      | `biller@test.com`             | `Biller1234!`           |

⚠ **Nota:** Estas cuentas son exclusivamente para pruebas. No las utilices en un entorno de producción.


## Descripcion General del Proyecto

## Seguridad en la autenticación y autorización dentro del sistema.

### Resumen de Roles y Permisos

| **Rol**               | **Pacientes** | **Médicos** | **Especialidades** | **Citas** | **Tratamientos** | **Facturación** |
|------------------------|---------------|-------------|--------------------|-----------|------------------|------------------|
| **Admin**      | CRUD*         | CRUD        | CRUD               | CRUD      | CRUD             | CRUD             |
| **Patient**           | R (propio)    | -           | -                  | R (propias)| -                | R (propias)      |
| **Doctor**             | R (pacientes) | R (propio)  | -                  | CRUD (propias)| CRUD (pacientes)| -                |
| **Receptionist**      | CRUD          | R           | R                  | CRUD      | -                | -                |
| **TreatmentSpecialist** | R          | R           | R                  | -         | CRUD             | -                |
| **Biller**         | R             | -           | -                  | -         | R                | CRUD             |


---

### Leyenda:
- **CRUD**: Permisos completos (Crear, Leer, Actualizar, Eliminar).
- **R**: Solo lectura.
- **-**: Sin acceso.
- **(propio)**: Acceso solo a su propia información.
- **(pacientes)**: Acceso solo a la información de los pacientes asociados.
- **(propias)**: Acceso solo a sus propias citas o facturas.
