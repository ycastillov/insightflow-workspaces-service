namespace InsightFlow.WorkspacesService.Src.Models
{
    /// <summary>
    /// Clase que representa un miembro de un espacio de trabajo.
    /// </summary>
    public class WorkspaceMember
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public required Guid UserId { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Rol del usuario en el espacio de trabajo.
        /// </summary>
        public required string Role { get; set; }

        /// <summary>
        /// Fecha en que el usuario se unió al espacio de trabajo.
        /// </summary>
        public DateTime JoinedAt { get; set; }
    }
}
