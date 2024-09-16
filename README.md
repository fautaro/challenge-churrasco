
# Challenge Churrasco


## Requerimientos desarrollados

Los requerimientos desarrollados en forma completa son los siguientes:

Inicio de sesión de usuario:
- Autenticación contra la base de datos proporcionada
- Autorización según rol de usuario (Claims)
- Sesión de usuario utilizando Cookie Authentication
- Manejo de cierre de sesión


Listado de productos:

- Queries para seleccionar productos activos en la tabla products.
- Modal para ver detalles del producto seleccionado (Para el caso de los productos con múltiples imágenes, se relevan las imagenes obtenidas en la ruta relativa definida en el campo image).
- Paginación de listado de productos.


Alta de producto

- Funcionalidad para carga de nuevo producto junto a la posibilidad de cargar multiples imágenes. (para esto, se guardan las imagenes en una ruta relativa del servidor)
## Authors

- [@fautaro](https://www.github.com/fautaro) - Lautaro Farias


## Arquitectura

Este challenge fue desarrollado bajo una arquitectura de separación de responsabilidades. Para esto, se crearon las siguientes capas:

- MVC (UI - Application): Se encarga de manejar la interfaz de usuario (UI) y la lógica de la aplicación. Para el manejo de la lógica de aplicación, se utilizan servicios (LoginService y ProductsService). 

- DataAccess (Infrastructure): Se encarga de gestionar la conexión a los orígenes de datos (bases de datos, servicios, etc). Esta capa incluye la lógica para realizar consultas a través de repositorios e interfaces. Estos últimos son utilizados desde la capa de Application (MVC).

- Domain (Entities): Contiene las entidades de la aplicación.

Idealmente, también se podría agregar una capa adicional (Application) para incluir la lógica de la aplicación. Sin embargo, en este challenge utilicé MVC tanto para la interfaz de usuario como para la capa de aplicación para simplificar el desarrollo.


## Librerías utilizadas

- Entity Framework Core
- AutoMapper
- Pomelo (MySql - MariaDB)

UI:

- JQuery
- Bootstrap
- Font Awesome
- Toastr (notificaciones)