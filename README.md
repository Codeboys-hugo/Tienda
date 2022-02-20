# Tienda

## Introducción

El proyecto contiene una base de datos local hecha con sql express con Code First, al iniciar el proyecto la base de datos se crea automaticamente.

### Preguntas 

1.- ¿Qué tipo de proyectos crearías en la aplicación WEB y cómo sería su estructura?

Proyectos donde se requiere la conexion de multiples personas y distintas locaciones.
Usaria MVC con WebApi donde todas las conexiones a base de datos y consulta de datos estarian del lado de los WebApi, la aplicacion mvc principal solo sera para mostrar informacion y contener logica de negocio de los clientes. Para asi con los WebApi poder compartir la información de terceros (partners) y con el proyecto mvc poder consumir informacion de esos terceros.

2.- Cuando cree que es más ventajoso usar una aplicación .NET MVC Core en lugar de ¿Usar un front-end independiente con una Web Api?

Cuando la información que mostrara o usara la  aplicación .NET MVC Core no sera compartida a otras aplicaciones. (solo esta misma web consultara la informacion y nadie más).

3.- ¿Cree que el uso de una estructura DDD debe aplicarse a cualquiertipo de proyectos? ¿Por qué?

Puede aplicarse, pero considero que si el proyecto es muy pequeño y no requerirá actualizaciones no es necesario.
