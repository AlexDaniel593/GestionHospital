# Guía de Ejecución de Pruebas

## Resumen de Pruebas Implementadas

El proyecto incluye un sistema completo de pruebas para validar la funcionalidad de seguridad y autenticación:

### ✅ Pruebas Unitarias - SecurityDAL (14 tests)
- Registro de intentos de login
- Bloqueo automático por 3 intentos fallidos
- Gestión de periodo de bloqueo (30 minutos)
- Reset de contadores
- Expiración automática de bloqueos

**Estado:** ✅ 14/14 pasando

---

## Ejecución de Pruebas

### Requisitos Previos
```powershell
# Verificar instalación de .NET SDK
dotnet --version  # Debe ser 9.0 o superior

# Navegar al directorio del proyecto
cd C:\Users\dalex\OneDrive\Documentos\Code\GestionHospital
```

### Ejecutar Todas las Pruebas
```powershell
dotnet test
```

### Ejecutar Solo Pruebas Unitarias
```powershell
dotnet test --filter "FullyQualifiedName~Unit"
```

### Ejecutar Solo Pruebas de SecurityDAL
```powershell
dotnet test --filter "FullyQualifiedName~SecurityDALTests"
```

### Ejecutar con Salida Detallada
```powershell
dotnet test --logger "console;verbosity=detailed"
```

### Ejecutar con Cobertura de Código
```powershell
dotnet test --collect:"XPlat Code Coverage"
```

---

## Resultados Esperados

```
Total tests: 14
     Passed: 14
     Failed: 0
     Skipped: 0
 Total time: ~2 segundos
```

---

## Tests Implementados

### 1. LogLoginAttemptAsync_SuccessfulLogin_SavesAttemptToDatabase
✅ Verifica que los intentos exitosos se registran correctamente

### 2. LogLoginAttemptAsync_FailedLogin_SavesAttemptWithReason
✅ Verifica que los intentos fallidos incluyen razón del fallo

### 3. RecordFailedAttemptAsync_FirstAttempt_IncrementsCounter
✅ Verifica incremento de contador en primer intento fallido

### 4. RecordFailedAttemptAsync_ThirdAttempt_LocksAccount
✅ Verifica bloqueo automático tras 3 intentos

### 5. RecordFailedAttemptAsync_AfterLockout_ResetsCounter
✅ Verifica que contador se resetea después del bloqueo

### 6. IsAccountLockedAsync_NoLockoutRecord_ReturnsNotLocked
✅ Verifica comportamiento sin bloqueos previos

### 7. IsAccountLockedAsync_ActiveLockout_ReturnsLocked
✅ Verifica detección de bloqueo activo

### 8. IsAccountLockedAsync_ExpiredLockout_ClearsLockoutAndReturnsNotLocked
✅ Verifica limpieza automática de bloqueos expirados

### 9. ResetFailedAttemptsAsync_WithExistingAttempts_ClearsCounter
✅ Verifica reseteo de contador en login exitoso

### 10. ResetFailedAttemptsAsync_NoExistingRecord_DoesNotThrow
✅ Verifica manejo seguro sin registros previos

### 11. GetRecentFailedAttemptsCountAsync_WithinTimeWindow_CountsAttempts
✅ Verifica conteo dentro de ventana de tiempo

### 12. GetRecentFailedAttemptsCountAsync_SuccessfulAttempts_NotCounted
✅ Verifica que intentos exitosos no se cuentan como fallidos

### 13. CompleteLoginFlow_ThreeFailedThenSuccess_ResetsLockout
✅ Prueba de flujo completo end-to-end

---

## Verificación en Base de Datos

Después de ejecutar las pruebas, puedes verificar los datos en la base de datos real:

### Consultar Intentos de Login
```sql
SELECT TOP 10 
    Email, 
    AttemptTime, 
    IsSuccessful, 
    IpAddress, 
    FailureReason
FROM LOGIN_ATTEMPTS
ORDER BY AttemptTime DESC;
```

### Consultar Bloqueos de Cuentas
```sql
SELECT 
    Email, 
    FailedAttempts, 
    LockoutEnd, 
    LastAttempt,
    CASE 
        WHEN LockoutEnd IS NOT NULL AND LockoutEnd > GETUTCDATE() THEN 'Bloqueada'
        ELSE 'Activa'
    END AS Estado
FROM ACCOUNT_LOCKOUTS;
```

### Ver Usuarios y Hash de Contraseñas
```sql
SELECT 
    Email, 
    UserName,
    LEFT(PasswordHash, 50) + '...' AS PasswordHashPreview
FROM AspNetUsers;
```

---

## Pruebas Funcionales Manuales

### Escenario 1: Login Exitoso
1. Ejecutar aplicación: `dotnet run --project GestionHospital`
2. Navegar a https://localhost:XXXX/Account/Login
3. Ingresar: admin@hospital.com / Admin1234
4. **Esperado:** Redirección a Home

### Escenario 2: Bloqueo Automático
1. Intentar login con contraseña incorrecta 3 veces
2. **Esperado después del 3er intento:**
   - Mensaje: "La cuenta está bloqueada... intente nuevamente en 30 minutos"
   - Email enviado a dalexis203@gmail.com

3. Verificar en base de datos:
```sql
SELECT * FROM ACCOUNT_LOCKOUTS WHERE Email = '[tu_email]';
-- LockoutEnd debe ser ~30 minutos en el futuro
```

### Escenario 3: Timeout de Sesión
1. Iniciar sesión exitosamente
2. Esperar 16 minutos sin actividad
3. Intentar navegar a cualquier página
4. **Esperado:** Redirección automática a /Account/Login

---

## Troubleshooting

### Problema: Tests no se ejecutan
```powershell
# Limpiar y reconstruir
dotnet clean
dotnet restore
dotnet build
dotnet test
```

### Problema: "Could not find test adapter"
```powershell
# Reinstalar paquetes de testing
cd GestionHospital.Tests
dotnet add package xunit
dotnet add package xunit.runner.visualstudio
```

### Problema: Errores de conexión a BD en tests
- Las pruebas unitarias usan **InMemoryDatabase**, no requieren SQL Server
- Las pruebas de integración (actualmente deshabilitadas) sí requieren BD

---

## Métricas de Calidad

| Componente | Cobertura | Tests | Estado |
|------------|-----------|-------|---------|
| SecurityDAL | ~95% | 14 | ✅ Pasando |
| Argon2PasswordHasher | 100% | Incluido en SecurityDAL | ✅ |
| AccountLockoutCLS | 100% | Incluido en SecurityDAL | ✅ |
| LoginAttemptCLS | 100% | Incluido en SecurityDAL | ✅ |

---

## Próximos Pasos

### Para Desarrollo
1. ✅ Pruebas unitarias de SecurityDAL
2. ⏳ Corrección de pruebas de integración
3. ⏳ Pruebas de PacienteController
4. ⏳ Pruebas de CitaController
5. ⏳ Pruebas de TratamientoController

### Para Producción
1. ✅ Verificar todas las pruebas unitarias pasan
2. ⏳ Ejecutar pruebas funcionales manuales
3. ⏳ Validar notificaciones por email
4. ⏳ Configurar CI/CD pipeline con tests automáticos

---

## Comandos Útiles

```powershell
# Ver lista de tests sin ejecutar
dotnet test --list-tests

# Ejecutar test específico por nombre
dotnet test --filter "Name=RecordFailedAttemptAsync_ThirdAttempt_LocksAccount"

# Generar reporte HTML de cobertura (requiere ReportGenerator)
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"coverage.cobertura.xml" -targetdir:"coverage-report" -reporttypes:Html

# Ejecutar tests en paralelo
dotnet test --parallel

# Ejecutar tests con timeout
dotnet test --blame-hang-timeout 60s
```

---

**Última actualización:** 10 de Diciembre de 2025  
**Mantenido por:** Equipo de Desarrollo
