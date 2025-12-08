namespace InsightFlow.WorkspacesService.Src.DTOs
{
    /// <summary>
    /// Clase que representa la solicitud para crear un espacio de trabajo.
    /// </summary>
    public class CreateWorkspaceRequest
    {
        /// <summary>
        /// Nombre del espacio de trabajo.
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// Descripción del espacio de trabajo.
        /// </summary>
        public required string Description { get; set; }

        /// <summary>
        /// Temática del espacio de trabajo.
        /// </summary>
        public required string Theme { get; set; }

        /// <summary>
        /// Imagen del espacio de trabajo.
        /// </summary>
        public required IFormFile Image { get; set; }
    }
}
