# MySwaggerModulo

## Descripcion 
Módulo para agregar la funcionalidad de swagger a la API con la opcion de Autenticarse

## Uso 
- Copiar los ficheros de esta capreta a alguna parte de la app
- Utilizar `builder.AddMySwagger();` y `app.UseMySwagger();` en *Program.cs*
- Para utilizar Propiedades personalizadas se puede crear en *appsettings.json* una seccion llamada MySwagger con los siguientes atributos:
  - Title 
  - Description 
  - Version 
  - Contact_Name
  - Contact_Url
  - **Ejemplo**:    

        "MySwagger": {
          "Title": "MiniAPI Modular",
          "Description": "Test de API Minima con Modulos",
          "Contact_Name": "My Name"
        }

## Requiere
- Swashbuckle.AspNetCore
    
        dotnet add package Swashbuckle.AspNetCore