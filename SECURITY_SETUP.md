# 🔐 Configuración de Seguridad - Gestión Hospitalaria

## Mejoras de Seguridad Implementadas

### ✅ 1. Hash de Contraseñas con Argon2id
- **Algoritmo**: Argon2id (ganador del Password Hashing Competition 2015)
- **Salt único**: Cada contraseña tiene un salt criptográficamente seguro de 128 bits
- **Parámetros de configuración**:
  - Memory: 64 MB
  - Iterations: 4
  - Parallelism: 2 threads
  - Hash size: 256 bits

### ✅ 2. Sistema de Bloqueo de Cuenta
- **Intentos permitidos**: 3 intentos fallidos
- **Período de bloqueo**: 30 minutos automáticos
- **Registro de intentos**: Todos los intentos (exitosos y fallidos) se registran en la tabla `LOGIN_ATTEMPTS`
- **Información registrada**:
  - Email del usuario
  - Timestamp del intento
  - Dirección IP
  - User Agent del navegador
  - Razón del fallo (si aplica)

### ✅ 3. Logging de Seguridad
- Todos los intentos fallidos se registran en:
  - Base de datos (tabla `LOGIN_ATTEMPTS`)
  - Logs de aplicación (ILogger)
- Información auditada:
  - IP de origen
  - Tiempo del intento
  - Razón del fallo
  - Estado de bloqueo

### ✅ 4. Configuración con Variables de Entorno
- **Credenciales removidas** del repositorio
- **Uso de archivo .env** para configuración local
- **.gitignore actualizado** para proteger datos sensibles

---

## 📋 Configuración Inicial

### 1. Clonar el Repositorio
```bash
git clone https://github.com/AlexDaniel593/GestionHospital.git
cd GestionHospital
```

### 2. Configurar Variables de Entorno

Copia el archivo de ejemplo y configura tus credenciales:

**En Windows (PowerShell):**
```powershell
Copy-Item GestionHospital\.env.example GestionHospital\.env
```

**En Linux/Mac:**
```bash
cp GestionHospital/.env.example GestionHospital/.env
```

Edita el archivo `.env` con tus credenciales:
```env
# Database Configuration
DB_SERVER=tu_servidor
DB_NAME=HospitalDB
DB_USER=tu_usuario
DB_PASSWORD=tu_contraseña

# Application Configuration
ASPNETCORE_ENVIRONMENT=Development
```

### 3. Restaurar Paquetes
```bash
dotnet restore
```

### 4. Aplicar Migraciones de Base de Datos

Ejecuta las migraciones para crear las tablas de seguridad:

```bash
dotnet ef database update --project CapaDatos --startup-project GestionHospital
```

Esto creará las siguientes tablas nuevas:
- `LOGIN_ATTEMPTS`: Registro de todos los intentos de login
- `ACCOUNT_LOCKOUTS`: Control de bloqueos de cuenta

### 5. Ejecutar la Aplicación
```bash
dotnet run --project GestionHospital
```

---

## 🗄️ Estructura de Tablas de Seguridad

### LOGIN_ATTEMPTS
| Campo | Tipo | Descripción |
|-------|------|-------------|
| Id | int | Identificador único |
| Email | nvarchar(256) | Email del usuario |
| AttemptTime | datetime2 | Timestamp del intento |
| IsSuccessful | bit | Indicador de éxito |
| IpAddress | nvarchar(45) | IP de origen |
| UserAgent | nvarchar(500) | Navegador/cliente |
| FailureReason | nvarchar(500) | Razón del fallo |

### ACCOUNT_LOCKOUTS
| Campo | Tipo | Descripción |
|-------|------|-------------|
| Id | int | Identificador único |
| Email | nvarchar(256) | Email del usuario (único) |
| FailedAttempts | int | Contador de intentos fallidos |
| LockoutEnd | datetime2 | Fin del período de bloqueo |
| LastAttempt | datetime2 | Último intento registrado |

---

## 🔍 Consultas Útiles de Seguridad

### Ver intentos de login fallidos recientes
```sql
SELECT TOP 20 
    Email, 
    AttemptTime, 
    IpAddress, 
    FailureReason
FROM LOGIN_ATTEMPTS
WHERE IsSuccessful = 0
ORDER BY AttemptTime DESC;
```

### Ver cuentas actualmente bloqueadas
```sql
SELECT 
    Email, 
    FailedAttempts, 
    LockoutEnd,
    DATEDIFF(MINUTE, GETUTCDATE(), LockoutEnd) AS MinutesRemaining
FROM ACCOUNT_LOCKOUTS
WHERE LockoutEnd IS NOT NULL 
  AND LockoutEnd > GETUTCDATE()
ORDER BY LockoutEnd DESC;
```

### Ver estadísticas de intentos fallidos por usuario
```sql
SELECT 
    Email,
    COUNT(*) AS TotalFailedAttempts,
    MAX(AttemptTime) AS LastFailedAttempt
FROM LOGIN_ATTEMPTS
WHERE IsSuccessful = 0
  AND AttemptTime >= DATEADD(DAY, -7, GETUTCDATE())
GROUP BY Email
ORDER BY TotalFailedAttempts DESC;
```

---

## ⚠️ Notas Importantes

1. **Archivo .env**: 
   - Nunca subas el archivo `.env` al repositorio
   - Cada desarrollador debe tener su propio archivo `.env` con sus credenciales locales
   - El archivo `.env.example` muestra la estructura necesaria

2. **Migraciones**:
   - Las migraciones están en `CapaDatos/Migrations/`
   - La migración `20251201000000_AddSecurityTables.cs` crea las tablas de seguridad

3. **Argon2id**:
   - El password hasher personalizado está en `CapaDatos/Argon2PasswordHasher.cs`
   - Se configura automáticamente en `Program.cs`

4. **Logging**:
   - Los logs de seguridad se escriben tanto en base de datos como en el sistema de logging de ASP.NET Core
   - Revisa los logs en desarrollo para monitorear actividad sospechosa

---

## 🚀 Despliegue en Producción

Cuando despliegues en producción:

1. **No uses archivo .env** - usa las variables de entorno del servidor
2. **Configura las variables** en tu proveedor de hosting (Azure, AWS, etc.)
3. **Asegura la conexión** con SSL/TLS
4. **Monitorea los logs** de intentos fallidos regularmente
5. **Considera alertas** para múltiples intentos fallidos desde la misma IP

---

## 📚 Referencias

- [Argon2 Password Hashing](https://github.com/P-H-C/phc-winner-argon2)
- [ASP.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity)
- [OWASP Authentication Cheat Sheet](https://cheatsheetseries.owasp.org/cheatsheets/Authentication_Cheat_Sheet.html)
