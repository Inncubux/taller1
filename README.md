# taller 1: E-Commerce

El siguiente código corresponde al taller número 1 el cual busca implementar un backend el cual cumpla la función de un comercio digital

integrates:

- Axel Mondaca Sanhueza 21.043.447-K
axel.mondaca@alumnos.ucn.cl
- Pedro Soto Ticona 24.161.653-3
pedro.soto02@alumnos.ucn.cl

## Requerimientos

- **[ASP.NET Core 8](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-9.0.203-windows-x64-installer)**
- **[Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)** 

## Clonar Repositorio

Clona el repositorio con el siguiente comando:


```bash
git clone https://github.com/Inncubux/taller1.git
```

## Restaurar el Proyecto

Después de clonar el repositorio, navega a la carpeta del proyecto y restaura los paquetes de NuGet:

```bash
cd taller1
dotnet restore
```

Crea el archivo `appsettings.json` para configurar la conexión a la base de datos y serilog. Agrega las siguientes lineas dentro del archivo creado y agrega una key de 64 bit en `SignInKey`:

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
  },
  "JWT": {
    "SignInKey": "",
    "Issuer": "https://localhost:7194/",
    "Audience": "https://localhost:7194/"
  },
  "AllowedOrigins": [
    "http://localhost"
  ]
  
}
  
```

Este ajuste se encargará de crear una base de datos local `store.db` y además de crear los logs.

## Ejecución del proyecto

Para la correcta ejecución del proyecto debemos realizar las migraciones a la base de datos con el siguiente comando:

```bash
dotnet ef migrations add initMigrations -o .\src\Data\Migrations\
dotnet ef database update
```

Para la ejecución del proyecto debemo ejecutar el siguiente comando:

```bash
dotnet run
```

Este se ejecutará en la siguiente url https://localhost:7194

