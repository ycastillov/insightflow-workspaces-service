namespace InsightFlow.WorkspacesService.Src.Models
{
    /// <summary>
    /// Clase que representa un espacio de trabajo.
    /// </summary>
    public class Workspace
    {
        /// <summary>
        /// Identificador único del espacio de trabajo.
        /// </summary>
        public required Guid Id { get; set; }

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
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Identificador público de la imagen en Cloudinary.
        /// </summary>
        public string ImagePublicId { get; set; } = string.Empty;

        /// <summary>
        /// Estad
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Recibe el identificador del usuario propietario del espacio de trabajo.
        /// </summary>
        public required Guid OwnerId { get; set; }

        /// <summary>
        /// Lista de miembros del espacio de trabajo.
        /// </summary>
        public List<WorkspaceMember> Members { get; set; } = new();
    }
}
