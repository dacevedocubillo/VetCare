# VetCare

Aplicación ASP.NET Core MVC para administrar una clínica veterinaria. Usa Entity Framework Core y SQLite.

## Módulos

- Propietarios
- Mascotas vinculadas a propietarios
- Citas vinculadas a mascotas
- Historiales médicos vinculados a mascotas

## Ejecutar

1. Abra `VetCare/VetCare.slnx` con Visual Studio 2026 o abra `VetCare/VetCare/VetCare.csproj`.
2. Confirme que tiene instalada la carga de trabajo **Desarrollo de ASP.NET y web**.
3. Ejecute el proyecto con `F5` o desde una terminal:

   ```powershell
   cd VetCare/VetCare
   dotnet run
   ```

Al iniciar, Entity Framework crea automáticamente el archivo `VetCare.db` y todas sus tablas a partir del modelo actual. No necesita instalar ni iniciar SQL Server. La cadena de conexión está en `appsettings.json`.

## Acceso inicial

- Usuario: `admin@vetcare.local`
- Contraseña temporal: `Cambiar123!`

Estas credenciales se crean solamente en el entorno de desarrollo. Cambie los valores de `Admin` en `appsettings.Development.json` antes del primer inicio. No existe registro público de administradores.

La página pública para clientes es `/reservar`. Utiliza un diseño independiente y no forma parte del panel administrativo. El panel, propietarios, mascotas, citas e historiales requieren iniciar sesión. Las nuevas solicitudes aparecen automáticamente en la agenda mediante SignalR.

## Correo y recordatorios

Configure `Smtp` en `appsettings.json`, variables de entorno o secretos de usuario. Se recomienda no guardar la contraseña real en el repositorio:

```powershell
cd VetCare/VetCare
dotnet user-secrets set "Smtp:Host" "smtp.su-proveedor.com"
dotnet user-secrets set "Smtp:Port" "587"
dotnet user-secrets set "Smtp:UserName" "correo@dominio.com"
dotnet user-secrets set "Smtp:Password" "contraseña-o-clave-de-aplicación"
dotnet user-secrets set "Smtp:FromEmail" "correo@dominio.com"
```

El sistema envía una confirmación al solicitar la cita y revisa cada cinco minutos las citas de las próximas 24 horas para enviar el recordatorio una sola vez.

## Estructura MVC

- `Models/`: entidades y reglas de validación.
- `Data/`: contexto de Entity Framework y migraciones.
- `Controllers/`: acciones HTTP y acceso a datos mediante el contexto.
- `Views/`: interfaz Razor organizada por controlador.
