# Modular Minimal Api

## Description
Este es un Test en desarrollo para crear una sistema para trabajar con Minimal Api de AspNet 6.0
----------
Lo que busco es crear un modelo para trabajar con Modulos reutilizables facilmente en otros proyectos

## Usage
- Tengo una Carpeta llamada AppServices que contiene la configuracion e implementacion de servicios basicos como Auth y Swagger
- En la carpeta Modules estan los modulos que se podrian reutilizar 
- Los modulos implementan una interfaz con dos metodos que crean los servicios necesarios para el funcionamiento del modulo y los endpoints que agrega
- La interfaz es IModule que esta en la raiz del proyecto. Esa interfaz la cogi de este [sitio](https://timdeschryver.dev/blog/maybe-its-time-to-rethink-our-project-structure-with-dot-net-6#a-domain-driven-api)

