# `InsightFlow-Workspaces-Service`

Este proyecto corresponde al **Workspaces Service** de la aplicación conceptual InsightFlow , que gestiona los espacios de trabajo colaborativos y sus miembros. Este servicio ha sido desarrollado bajo una arquitectura de microservicios con un fuerte enfoque en la automatización del *pipeline* de CI/CD.

## 1. 🏗️ Arquitectura y Diseño 
### Arquitectura General 
El servicio forma parte de una arquitectura de **Microservicios**, donde cada servicio es independiente, desplegable individualmente y comunica sus datos a través de identificadores (GUIDs) y peticiones HTTP síncronas a otros servicios (ej. Users Service) cuando es necesario complementar información. 

### Patrón de Diseño Empleado
* **Patrón de Diseño:** **Arquitectura por Capas** (Clean Architecture simplificada) con enfoque en el **Patrón Repository**. 


* **Capas:**
  * **Controller:** Expone los *endpoints* HTTP y maneja las solicitudes.
  * **DTOs:** Define las estructuras de datos de entrada/salida (`Request` y `Response`).
  * **Repository:** Simula la capa de persistencia de datos (diccionario en memoria).
  * **Services:** Servicios externos (ej. `PhotoService` para Cloudinary).



### Tecnologías
| Categoría | Tecnología/Framework | Notas |
| --- | --- | --- |
| **Backend** | C# .NET Web API | Framework para el desarrollo del servicio.|
| **Persistencia** | `ConcurrentDictionary<Guid, Workspace>` | **Datos en Memoria**. No se usa Base de Datos real, se reinicia con cada despliegue. |
| **Archivos** | Cloudinary | Utilizado para alojar la imagen/ícono del *workspace*. |
| **Validación** | FluentValidation | Para validación de DTOs y reglas de negocio. |
| **Mapeo** | AutoMapper | Para la conversión eficiente entre Modelos y DTOs. |

## 2. 🔌 Endpoints Disponibles (API)
El servicio se encuentra desplegado en Render (https://insightflow-workspaces.onrender.com) y expone los siguientes *endpoints*. La ruta base es `/api/workspaces`.

| Método | Ruta | Descripción | Requisitos de Rol |
| --- | --- | --- | --- |
| **`POST`** | `/api/workspaces/{ownerId}` | Crea un nuevo espacio. Requiere `ownerId` en la ruta y datos del espacio, incluyendo una imagen obligatoria (`IFormFile`). | N/A (Usuario autenticado) |
| **`GET`** | `/api/workspaces/user/{userId}` | Retorna un listado de todos los espacios donde el `userId` es miembro. | Usuario logueado. |
| **`GET`** | `/api/workspaces/{id}` | Retorna la información detallada de un espacio (incluyendo lista de miembros). Requiere que el usuario consultante sea miembro. | Miembro del espacio |
| **`PATCH`** | `/api/workspaces/{id}` | Actualiza campos del espacio (`Name`, `Image`). Maneja el reemplazo/eliminación de la imagen anterior en Cloudinary. | **Propietario** (o Administrador) |
| **`DELETE`** | `/api/workspaces/{id}` | Aplica **Soft Delete** al espacio, marcándolo como inactivo para preservar la trazabilidad. | **Propietario** |

### 🚨 Nota Importante sobre Soft Delete
La eliminación de espacios y tareas/documentos en InsightFlow se realiza mediante **SOFT DELETE**, garantizando que las contribuciones pasadas no se pierdan ni generen referencias rotas.

## 3. 💾 Data Seeder (Datos en Memoria)
Para cumplir con el requerimiento de tener datos de ejemplo , el `WorkspaceRepository` incluye un **Seeder** que inicializa dos espacios de trabajo y un usuario de prueba al arrancar el servicio: 

* **Usuario de Prueba/Seeder ID:** `d084f70c-238d-44a3-a7d0-1a7795325c34` (Este ID se usa para las pruebas iniciales y se simula como el usuario logueado en la mayoría de los *endpoints*).
* **Datos Cargados:** Dos espacios (`Proyecto Alpha`, `Documentación Interna`) donde el usuario de prueba es miembro o propietario.

## 4. 🚀 Flujo de CI/CD (DevOps)
El foco principal de este taller es la automatización del despliegue. El servicio implementa un *pipeline* de Despliegue Continuo (CD) completo a través de GitHub Actions, Docker Hub y Render.

| Etapa | Herramienta | Acción | Resultado |
| --- | --- | --- | --- |
| **Trigger** | GitHub | `push` o `merge` a la rama `main`. | Dispara el *workflow*. |
| **CI: Build & Push** | GitHub Actions & Docker Hub | Construye la imagen Docker del servicio y la publica (`push`) en Docker Hub con el tag `latest`. | La nueva imagen está lista en el registro. |
| **CD: Deploy** | GitHub Actions & Render | Llama al *Deploy Webhook* de Render. | Render jala la nueva imagen desde Docker Hub y relanza el servicio, completando el despliegue automático. |

## 5. 🧑‍💻 Cómo Ejecutar el Proyecto (Localmente)
### Prerrequisitos
* .NET SDK (versión 8.0 o superior).
* Docker (opcional, si desea ejecutarlo como contenedor localmente).
* Postman (para probar los *endpoints*).
* Cuenta de Cloudinary (para subir archivos) con las credenciales configuradas en `appsettings.json`. (Ya incluido en appsettings para este taller)

### Instrucciones
1. **Clonar el Repositorio:**
```bash
git clone https://github.com/ycastillov/insightflow-workspaces-service.git
cd insightflow-workspaces-service
```
2. **Instalar Dependencias:**
```bash
dotnet restore
```
3. **Ejecutar el Servicio:**
```bash
dotnet run
```
El servicio se iniciará en `http://localhost:5250` (o el puerto configurado en `launchSettings.json`).

4. **Pruebas con Postman:**
* Utilice el **Usuario de Prueba/Seeder ID** (`d084f70c-238d-44a3-a7d0-1a7795325c34`) para probar el *endpoint* de listado: `GET http://localhost:5250/api/workspaces/user/d084f70c-238d-44a3-a7d0-1a7795325c34`.
