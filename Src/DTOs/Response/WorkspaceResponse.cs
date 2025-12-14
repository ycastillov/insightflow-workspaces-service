namespace InsightFlow.WorkspacesService.Src.DTOs.Response
{
    /// <summary>
    /// DTO que representa la respuesta de un espacio de trabajo.
    /// </summary>
    public class WorkspaceResponse
    {
        /// <summary>
        /// ID del espacio de trabajo.
        /// </summary>
        public string? Id { get; set; }

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

        // Requerido: Rol del usuario autenticado en el espacio (se asigna en el controlador)
        public required string Role { get; set; }

        // Requerido: Lista completa de miembros con sus ID, nombres y roles
        public required List<WorkspaceMemberResponse> Members { get; set; } = [];
    }
}
