# 🏥 Gestión Hospitalaria  

📌 **Proyecto de Programación Web - Parcial 3**  

## 👥 Integrantes  
- **Daniel Guaman**  
- **Kevin Amaguana**  

🚀 Desarrollo de una plataforma web para la gestión hospitalaria, incluyendo administración de usuarios, asignación de roles y manejo de citas médicas.  


> [!IMPORTANT]
> # Cuentas de Prueba
>
> A continuación, encontrarás las credenciales de prueba para los distintos roles del sistema.  
> Úsalas para acceder y probar las funcionalidades según el rol asignado.

### 🔑 Credenciales de Prueba  

| Rol                  | Email                           | Contraseña              |
|----------------------|--------------------------------|-------------------------|
| **Admin**    | `admin@admin.com`             | `Admin1234,`             |
| **Doctor**          | `adguaman29@gmail.com`             | `Dguaman2025!`           |
| **Staff**   | `receptionist@test.com`       | `Receptionist1234!`     |
| **Patient**   | `sdlasso@gmail.com`       | `Slasso2003!`     |




## Seguridad en la autenticación y autorización dentro del sistema.

### Resumen de Roles y Permisos

| **Rol**               | **Pacientes** | **Médicos** | **Especialidades** | **Citas** | **Tratamientos** | **Facturación** |
|------------------------|---------------|-------------|--------------------|-----------|------------------|------------------|
| **Admin**      | CRUD*         | CRUD        | CRUD               | CRUD      | CRUD             | CRUD             |
| **Patient**           | R (propio)    | -           | -                  | R (propias)| -                | R (propias)      |
| **Doctor**             | R (pacientes) | R (propio)  | -                  | RUD (propias)| CRUD (pacientes)| -                |
| **Staff**      | CRUD          | R           | R                  | CRUD      | CRUD                | CRUD                |


---

### Leyenda:
- **CRUD**: Permisos completos (Crear, Leer, Actualizar, Eliminar).
- **R**: Solo lectura.
- **-**: Sin acceso.
- **(propio)**: Acceso solo a su propia información.
- **(pacientes)**: Acceso solo a la información de los pacientes asociados.
- **(propias)**: Acceso solo a sus propias citas o facturas.


## Gestión de Pacientes

### Guardar Paciente

Al llamar al método `GuardarPaciente(paciente)`, se realizarán las siguientes acciones:

1. **Guardar el paciente** en la base de datos.
2. **Generar la contraseña** para el paciente (por ejemplo, `Jperez1990!`).
3. **Crear un usuario** en la tabla `AspNetUsers` con el **email** `juan.perez@example.com` y la **contraseña** `Jperez1990!`.
4. **Asignar el rol** "Patient" al usuario.

### Sincronización de Pacientes Manualmente Ingresados

Si se ingresan nuevos pacientes directamente en la base de datos (por ejemplo, mediante un gestor de bases de datos o scripts SQL), será necesario crear las cuentas de usuario correspondientes en la aplicación.

Para sincronizar estos pacientes, puedes ejecutar el siguiente endpoint:

https://dominio/Paciente/CrearCuentasParaPacientesExistentes

Este endpoint realizará lo siguiente:

- Recorrerá todos los registros de la tabla de **pacientes** que tengan el campo `BHABILITADO = 1`.
- Generará una **contraseña** para cada paciente utilizando la función `GenerarContrasena` definida en `AccountController`.
- Creará un **usuario** en la tabla `AspNetUsers` y asignará el rol **Patient** a cada cuenta.

>[!IMPORTANT]  
> Esta acción está diseñada para ejecutarse **una sola vez**. Si se ejecuta nuevamente, podría intentar crear duplicados en la base de datos.
>
> Por lo tanto, se recomienda:
> - Ejecutar este endpoint **solo cuando se hayan agregado pacientes manualmente** a la base de datos.

---

# Proyecto de Gestión Hospitalaria en ASP.NET Core MVC

## Introducción

Aqui se describe el desarrollo de un sistema web de gestión hospitalaria diseñado para administrar la información de pacientes, médicos, especialidades médicas, citas, tratamientos y facturación. La implementación está basada en ASP.NET Core MVC, lo que proporciona una arquitectura robusta y escalable para facilitar la administración y mantenimiento del sistema.

## Objetivos del Proyecto

- Desarrollar una plataforma web para la gestión de pacientes, médicos y citas médicas.
- Implementar una interfaz de usuario moderna y responsiva utilizando Bootstrap.
- Proporcionar un sistema seguro con autenticación y autorización basada en roles.
- Desplegar el sistema en la nube utilizando un servicio gratuito de hosting como Somee.com.

## 1. Requisitos Previos

Antes de comenzar con la implementación del sistema, es necesario contar con las siguientes herramientas y programas:

- Visual Studio 2022 con soporte para .NET Core.
- SQL Server y SQL Server Management Studio (SSMS).
- Bootstrap o Materialize CSS para la creación de una interfaz moderna y responsiva.
- JavaScript para la mejora de la interactividad del sistema.

## 2. Descripción del Proyecto

El sistema permitirá gestionar los siguientes módulos:

- **Pacientes**: Registro, actualización y consulta de pacientes.
- **Médicos**: Administración de la información de los médicos.
- **Especialidades**: Gestión de especialidades médicas disponibles.
- **Citas**: Programación y gestión de citas médicas.
- **Tratamientos**: Información sobre tratamientos aplicados a pacientes.
- **Facturación**: Generación de facturas y consultas sobre pagos realizados.

La arquitectura del sistema se basa en ASP.NET Core MVC, lo que proporciona una separación clara entre las capas de presentación, negocio y acceso a datos. Además, se utiliza Entity Framework Core para la interacción con la base de datos.

## 3. Creación de la Base de Datos

El sistema se apoya en una base de datos relacional en SQL Server, que incluye las siguientes tablas:

- **Pacientes**: Información personal y médica de los pacientes.
- **Médicos**: Datos de los médicos registrados en el hospital.
- **Especialidades**: Listado de especialidades médicas disponibles.
- **Citas**: Registro de las citas médicas programadas.
- **Tratamientos**: Información sobre los tratamientos aplicados a los pacientes.
- **Facturación**: Registro de los pagos y facturas generadas.


