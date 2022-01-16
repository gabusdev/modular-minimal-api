# AuthenticationUtilsModule

## Description
>Este modulo trae funciones utiles que se pueden utilizar en la gestion de Autenticar usuarios   
>Y crear Tokens de seguridad con el mecanismo de *Jwt Bearer*

## Usage
- **Importante** Se Requiere una configuracion en *appsettings.json* con kew "Jwt" y los siguentes atributos:
  - "Issuer"
  - "Audience"
  - "Key"
  - "Secret"
  - "ExpireTime"
- Copiar el modulo y agregarlo al DI para usarlo en cualquier otro modulo

        using Modules.AuthenticationUtilsModule.Services;

        services.AddTransient<IAuthenticationService,AuthenticationService>();

### Que hace hasta ahora
- MakeHash
- GenerateToken
- VerifyHash

## Requires
- Microsoft.AspNetCore.Authentication.JwtBearer
        
        dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer