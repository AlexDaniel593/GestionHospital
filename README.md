# GestionHospital
Programacion Web Proyecto Parcial 3
Integrantes:
- Daniel Guaman
- Kevin Amaguana

> .[!IMPORTANT]
># Cuentas de Prueba

> A continuación se detallan las cuentas de prueba disponibles para diferentes roles en el sistema. Utiliza estas credenciales para acceder a las funcionalidades específicas de cada rol.
>- **Admin**: 
>  - **Email**: admin@admin.com
>  - **Password**: Admin1234,
>- **Doctor**: 
>  - **Email**: doctor@test.com
>  - **Password**: Doctor1234!
>- **Receptionist**: 
>  - **Email**: receptionist@test.com
>  - **Password**: Receptionist1234!
>- **TreatmentSpecialist**: 
>  - **Email**: treatmentspecialist@test.com
>  - **Password**: TreatmentSpecialist1234!
>- **Biller**: 
>  - **Email**: biller@test.com
>  - **Password**: Biller1234!

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
