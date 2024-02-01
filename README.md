# API de Transacciones Financieras

API diseñada para gestionar transacciones financieras como tarjetas de crédito, pagos, compras y estados de cuenta

## Endpoints Disponibles

### CreditCard

```**GET /api/CreditCard/List**```
- Obtiene la lista de tarjetas de crédito.

**POST /api/CreditCard/Search**
- Busca tarjetas de crédito según un número proporcionado.

### Payment

**POST /api/Payment/Create**
- Crea un nuevo pago.

**GET /api/Payment/Get/{ClientId}**
- Obtiene los pagos asociados a un cliente por su ID.

### PaymentInfo

**GET /api/PaymentInfo/Get/{ClientId}**
- Obtiene la información de pagos asociada a un cliente por su ID.

### Purchase

**POST /api/Purchase/Create**
- Crea una nueva compra.

**GET /api/Purchase/Get/{ClientId}**
- Obtiene las compras asociadas a un cliente por su ID.

### PurchaseInfo

**GET /api/PurchaseInfo/Get/{ClientId}**
- Obtiene la información de compras asociada a un cliente por su ID.

### Statement

**GET /api/Statement/List**
- Obtiene la lista de estados de cuenta.

**GET /api/Statement/Get/{ClientId}**
- Obtiene el estado de cuenta de un cliente por su ID.
```
## Uso

Para utilizar la API, realiza solicitudes HTTP a los endpoints mencionados, proporcionando los datos necesarios en el cuerpo de la solicitud (para métodos POST) o como parámetros de la URL.

# Frontend de Transacciones Financieras

## Frontend Blazor WebAssembly

### Instalación

Asegúrate de tener [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) instalado.

```bash
dotnet build
dotnet run
```
### Configuración de la Base de Datos
El string de conexión a la base de datos se encuentra en el archivo appsettings.json. Puedes modificar la cadena SQL en la sección ConnectionStrings para adaptarla a tu entorno:
{
  "ConnectionStrings": {
    "CadenaSQL": "Data Source=TuServidor; Initial Catalog=TuBaseDeDatos; User Id=TuUsuario; Pwd=TuContraseña; TrustServerCertificate=True; Encrypt=False;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
### Configuracion de la API en Frontend
La URL de la API se configura en el archivo program.cs. Puedes modificar la dirección base de la API ajustando la propiedad BaseAddress del cliente HTTP en la siguiente sección:

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7106/api/") });

## Paquetes NuGet

Este proyecto hace uso de varios paquetes NuGet para facilitar el desarrollo y mejorar la funcionalidad. A continuación, se enumeran los paquetes utilizados junto con sus versiones:

- **AutoMapper** (v12.0.1) - Librería para mapeo de objetos.
- **AutoMapper.Extensions.Microsoft.DependencyInjection** (vX.X.X) - Integración de AutoMapper con el contenedor de inyección de dependencias de Microsoft.
- **FluentValidation** (v11.9.0) - Biblioteca para validación de modelos.
- **MudBlazor** (v6.14.0) - Componentes y estilos de interfaz de usuario para Blazor.
- **Microsoft.Data.SqlClient** (v5.1.4) - Proveedor de datos para SQL Server.

Recuerda verificar el archivo `*.csproj` para obtener la lista completa de paquetes y sus versiones.




