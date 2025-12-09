using InsightFlow.WorkspacesService.Src.Models;

namespace InsightFlow.WorkspacesService.Src.Interfaces
{
    /// <summary>
    /// Interfaz para el repositorio de espacios de trabajo.
    /// </summary>
    public interface IWorkspaceRepository
    {
        /// <summary>
        /// Crea un nuevo espacio de trabajo.
        /// </summary>
        /// <param name="workspace">El espacio de trabajo a crear.</param>
        /// <returns>El espacio de trabajo creado.</returns>
        Task<Workspace> CreateAsync(Workspace workspace);

        /// <summary>
        /// Obtiene un espacio de trabajo por su identificador.
        /// </summary>
        /// <param name="id">El identificador del espacio de trabajo.</param>
        /// <returns>El espacio de trabajo si se encuentra; de lo contrario, null.</returns>
        Task<Workspace?> GetByIdAsync(Guid id);

        /// <summary>
        /// Obtiene todos los espacios de trabajo asociados a un usuario.
        /// </summary>
        /// <param name="userId">El identificador del usuario.</param>
        /// <returns>Una colección de espacios de trabajo asociados al usuario.</returns>
        Task<IEnumerable<Workspace>> GetByUserIdAsync(Guid userId);

        /// <summary>
        /// Actualiza un espacio de trabajo existente.
        /// </summary>
        /// <param name="workspace">El espacio de trabajo a actualizar.</param>
        /// <returns>El espacio de trabajo actualizado.</returns>
        Task<Workspace> UpdateAsync(Workspace workspace);

        /// <summary>
        /// Marca un espacio de trabajo como eliminado de forma lógica.
        /// </summary>
        /// <param name="id">El identificador del espacio de trabajo a eliminar.</param>
        Task SoftDeleteAsync(Guid id);

        /// <summary>
        /// Verifica si existe un espacio de trabajo con el mismo nombre.
        /// </summary>
        /// <param name="name">El nombre del espacio de trabajo.</param>
        /// <param name="excludeId">El identificador del espacio de trabajo a excluir de la verificación (opcional).</param>
        /// <returns>True si existe un espacio de trabajo con el mismo nombre; de lo contrario, false.</returns>
        Task<bool> ExistsWithNameAsync(string name, Guid? excludeId = null);

        /// <summary>
        /// Verifica si existe un espacio de trabajo con el mismo id.
        /// </summary>
        /// <param name="id">El identificador del espacio de trabajo.</param>
        /// <returns>True si existe un espacio de trabajo con el mismo id; de lo contrario, false.</returns>
        Task<bool> ExistsByIdAsync(Guid id);
    }
}
