namespace InsightFlow.WorkspacesService.Src.DTOs.Response
{
    /// <summary>
    /// DTO que representa la respuesta de un espacio de trabajo.
    /// </summary>
    public class WorkspaceResponse
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
        /// URL de la imagen del espacio de trabajo.
        /// </summary>
        public required string ImageUrl { get; set; }
    }
}
