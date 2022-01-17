# MyAuthModule

## Description
En este modulo agrega una autenticacion y autorizacion con el esquema JwtBearer en una WebApi.   
Automaticamente agrega una política de solicitud de autorizacion a todos los Endpoints.     
Es necesario permitir el `AllowAnonymous` a los endpoints que lo requieran.
## Usage
- Simplemente copiar el modulo a la app
- Utilizar `builder.AddMyAuth();` y `app.UseMyAuth();` en Program.cs
- ***Importante:*** Es necesario configrar en el *appsettings.json* los siguientes opciones dentro de la clave *Jwt*
  - Issuer
  - Audience
  - Key (La Key debe ser generada automaticamente con longitud recomendada no menor que 16 caracteres)
  - Secret
  - **Ejemplo**:

        "Jwt": {
          "Issuer": "https://localhost:6060",
          "Audience": "https://localhost:6060",
          "Key": "Yrt47SB56Rt39OlpJYt5642E39iaUYt2"
          "Secret": "Yrt47SB56Rt39O"
        }
- Siempre Utilizar `app.UseMyAuth();` primero que Mapear los Endpoints.
## Requires
- Microsoft.AspNetCore.Authentication.JwtBearer
    
        dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer