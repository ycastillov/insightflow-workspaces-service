namespace InsightFlow.WorkspacesService.Src.DTOs
{
    /// <summary>
    /// DTO que representa la solicitud para actualizar un espacio de trabajo.
    /// </summary>
    public class UpdateWorkspaceRequest
    {
        /// <summary>
        /// Nombre del espacio de trabajo.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Descripción del espacio de trabajo.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Temática del espacio de trabajo.
        /// </summary>
        public string? Theme { get; set; }

        /// <summary>
        /// Imagen del espacio de trabajo.
        /// </summary>
        public IFormFile? Image { get; set; }
    }
}
