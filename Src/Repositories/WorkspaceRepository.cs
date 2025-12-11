using System.Collections.Concurrent;
using InsightFlow.WorkspacesService.Src.Interfaces;
using InsightFlow.WorkspacesService.Src.Models;

namespace InsightFlow.WorkspacesService.Src.Repositories
{
    public class WorkspaceRepository : IWorkspaceRepository
    {
        // Usamos ConcurrentDictionary para simular la BD en memoria (hilo seguro)
        // Key: Workspace ID (UUID V4)
        private static readonly ConcurrentDictionary<Guid, Workspace> _workspaces = new();

        // Simulación de un usuario que se usará para el seeder y pruebas
        private static readonly Guid _testUserId = new("d084f70c-238d-44a3-a7d0-1a7795325c34");

        public WorkspaceRepository()
        {
            // Seed de datos al inicializar el repositorio
            if (_workspaces.IsEmpty)
            {
                SeedData();
            }
        }

        // --- SEEDER DE DATOS (Requerido para probar el CRUD) ---
        private void SeedData()
        {
            var workspaceId1 = Guid.NewGuid();
            var workspaceId2 = Guid.NewGuid();
            var now = DateTime.UtcNow;

            // Workspace 1: Propiedad del usuario de prueba
            var ws1 = new Workspace
            {
                Id = workspaceId1,
                Name = "Proyecto Alpha",
                Description = "Espacio para el desarrollo del MVP",
                Theme = "Tecnología",
                ImageUrl = "http://example.com/alpha.png",
                ImagePublicId = "alpha_public_id",
                OwnerId = _testUserId, // Usuario Propietario
                IsActive = true,
                Members = new List<WorkspaceMember>
                {
                    new WorkspaceMember
                    {
                        UserId = _testUserId,
                        UserName = "TestUser Owner",
                        Role = "Propietario",
                    },
                },
            };

            // Workspace 2: Un usuario anexo (simulando que el test user es Editor)
            var ws2OwnerId = Guid.NewGuid();
            var ws2 = new Workspace
            {
                Id = workspaceId2,
                Name = "Documentación Interna",
                Description = "Archivos legales y HR",
                Theme = "Legal",
                ImageUrl = "http://example.com/docs.png",
                ImagePublicId = "docs_public_id",
                OwnerId = Guid.NewGuid(), // Otro propietario
                IsActive = true,
                Members = new List<WorkspaceMember>
                {
                    new WorkspaceMember
                    {
                        UserId = ws2OwnerId,
                        UserName = "Admin Legal",
                        Role = "Propietario",
                    },
                    new WorkspaceMember
                    {
                        UserId = _testUserId,
                        UserName = "TestUser Owner",
                        Role = "Editor",
                    },
                },
            };

            _workspaces.TryAdd(ws1.Id, ws1);
            _workspaces.TryAdd(ws2.Id, ws2);
        }

        public Task<Workspace> CreateAsync(Workspace workspace)
        {
            // Aseguramos que el ID se genera antes de que se llame este método desde el servicio,
            // pero lo validamos para no sobreescribir si ya existe (aunque en el CRUD de la API se evitará)
            if (workspace.Id == Guid.Empty)
            {
                workspace.Id = Guid.NewGuid(); // Generamos el UUID V4 si es necesario
            }

            // Simular el comportamiento de una BD al insertar
            _workspaces.TryAdd(workspace.Id, workspace);
            return Task.FromResult(workspace);
        }

        public Task<Workspace?> GetByIdAsync(Guid id)
        {
            // Busca por ID y solo devuelve si está activo (Soft Delete)
            _workspaces.TryGetValue(id, out var workspace);
            return Task.FromResult(workspace?.IsActive == true ? workspace : null);
        }

        public Task<IEnumerable<Workspace>> GetByUserIdAsync(Guid userId)
        {
            // Filtra los espacios donde el usuario es miembro y están activos
            var userWorkspaces = _workspaces
                .Values.Where(ws => ws.IsActive && ws.Members.Any(m => m.UserId == userId))
                .ToList();

            return Task.FromResult<IEnumerable<Workspace>>(userWorkspaces);
        }

        public Task<Workspace> UpdateAsync(Workspace workspace)
        {
            // El ConcurrentDictionary no tiene un método Update directo, usamos TryUpdate.
            // Primero se lee el antiguo, pero el DTO de actualización ya debe haber mapeado
            // los campos actualizados en la instancia que recibimos.

            // Simulación simple de actualización: si existe, se reemplaza.
            _workspaces[workspace.Id] = workspace;

            return Task.FromResult(workspace);
        }

        public Task SoftDeleteAsync(Guid id)
        {
            if (_workspaces.TryGetValue(id, out var workspace))
            {
                // Implementación del SOFT DELETE, marcando como inactivo
                workspace.IsActive = false;
                // No lo removemos del diccionario para preservar la trazabilidad
            }
            return Task.CompletedTask;
        }

        public Task<bool> ExistsWithNameAsync(string name, Guid? excludeId = null)
        {
            // Validación de nombre único, excluyendo el propio ID durante la edición
            var exists = _workspaces.Values.Any(ws =>
                ws.IsActive
                && ws.Name.Equals(name, StringComparison.OrdinalIgnoreCase)
                && ws.Id != excludeId
            );

            return Task.FromResult(exists);
        }

        public Task<bool> ExistsByIdAsync(Guid id)
        {
            // Verifica si el espacio existe y está activo (Soft Delete)
            return Task.FromResult(_workspaces.ContainsKey(id) && _workspaces[id].IsActive);
        }
    }
}
