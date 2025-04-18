# taller 1: E-Commerce

## Requerimientos

- **[ASP.NET Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-9.0.203-windows-x64-installer)**
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)** 

## Clonar Repositorio

Clona el repositorio con el siguiente comando:


```bash
git clone https://github.com/IDWM/dotnet-exam1.git
```

## Restaurar el Proyecto

Después de clonar el repositorio, navega a la carpeta del proyecto y restaura los paquetes de NuGet:

```bash
cd taller1
dotnet restore
```

Crea el archivo `appsettings.json` para configurar la conexión a la base de datos y serilog. Agrega las siguientes lineas dentro del archivo creado:

```json
{
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    }, 
    "ConnectionStrings": {
      "DefaultConnection": "Data source=store.db"
    },
    "AllowedHosts": "*",
  
    
    "Serilog": {
      "MinimumLevel": {
        "Default": "Information",
        "Override": {
          "Microsoft": "Warning",
          "Microsoft.AspNetCore": "Warning",
          "Microsoft.AspNetCore.Hosting.Diagnostics": "Error",
          "Microsoft.Hosting.Lifetime": "Error",
          "System": "Warning"
        }
      },
        "Enrich": [ "FromLogContext", "WithMachineName", "WithThreadId" ],
        "WriteTo": [
          { "Name": "Console" },
          {
            "Name": "File",
            "Args": {
              "path": "Logs/log-.txt",
              "rollingInterval": "Day",
              "restrictToMinimumLevel": "Information"
            }
          }
        ]
  }
}
  
```

Este ajuste se encargará de crear una base de datos local `store.db` y además de crear los logs.


