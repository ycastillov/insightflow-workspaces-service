using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightFlow.WorkspacesService.Src.DTOs.Response
{
    /// <summary>
    /// DTO para la respuesta de un ítem en la lista de espacios de trabajo
    /// </summary>
    public class WorkspaceListItemResponse
    {
        /// <summary>
        /// Identificador único del espacio de trabajo.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Nombre del espacio de trabajo.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Imagen del espacio de trabajo.
        /// </summary>
        public string ImageUrl { get; set; } = string.Empty;

        /// <summary>
        /// Rol del usuario en el espacio de trabajo.
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// Fecha en que el usuario se unió al espacio de trabajo.
        /// </summary>
        /// <value></value>
        public DateTime JoinedAt { get; set; }
    }
}
