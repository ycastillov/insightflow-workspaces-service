using AutoMapper;
using CloudinaryDotNet.Actions;
using InsightFlow.WorkspacesService.Src.DTOs;
using InsightFlow.WorkspacesService.Src.DTOs.Response;
using InsightFlow.WorkspacesService.Src.Interfaces;
using InsightFlow.WorkspacesService.Src.Models;
using Microsoft.AspNetCore.Mvc;

namespace InsightFlow.WorkspacesService.Src.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Ruta base: /api/workspaces
    public class WorkspacesController(
        IWorkspaceRepository repository,
        IMapper mapper,
        IPhotoService photoService
    ) : ControllerBase
    {
        private readonly IWorkspaceRepository _repository = repository;
        private readonly IMapper _mapper = mapper;
        private readonly IPhotoService _photoService = photoService;

        // Constructor con Inyección de Dependencias

        [HttpPost("{ownerId}")]
        public async Task<ActionResult<WorkspaceResponse>> CreateWorkspace(
            Guid ownerId,
            [FromForm] CreateWorkspaceRequest request
        )
        {
            // 1. Validaciones
            if (request.Image == null)
            {
                // La imagen es obligatoria
                return BadRequest(
                    new
                    {
                        Message = "Se requiere un archivo de imagen para crear el espacio de trabajo (Ícono).",
                    }
                );
            }

            if (await _repository.ExistsWithNameAsync(request.Name))
            {
                return BadRequest(new { Message = "El nombre del espacio de trabajo ya existe." });
            }

            // 2. Subir el archivo de imagen
            ImageUploadResult uploadResult;
            try
            {
                uploadResult = await _photoService.AddPhotoAsync(request.Image);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }

            if (uploadResult.Error != null)
                return StatusCode(500, new { Message = "Error al subir la imagen a Cloudinary." });

            // 3. Mapear y Asignar
            var workspace = _mapper.Map<Workspace>(request);
            workspace.Id = Guid.NewGuid(); // Asignar UUID V4
            workspace.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
            workspace.ImagePublicId = uploadResult.PublicId; // Almacenamos el PublicId para la eliminación futura

            // 4. Asignar propietario (usando el ownerId del segmento de la URL)
            workspace.OwnerId = ownerId;
            workspace.Members.Add(
                new WorkspaceMember
                {
                    UserId = ownerId,
                    UserName = $"User-{ownerId.ToString().Substring(0, 4)}", // Simulación
                    Role = "Propietario",
                    JoinedAt = DateTime.UtcNow,
                }
            );

            // 5. Persistir
            await _repository.CreateAsync(workspace);

            // 6. Retornar Respuesta (Necesitamos construir la WorkspaceResponse completa)
            var response = _mapper.Map<WorkspaceResponse>(workspace);
            response.Role = "Propietario"; // El creador siempre es propietario
            response.Members = _mapper.Map<List<WorkspaceMemberResponse>>(workspace.Members);

            return CreatedAtAction(nameof(GetWorkspaceById), new { id = workspace.Id }, response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkspaceListItemResponse>>> GetWorkspaces(
            Guid userId
        )
        {
            var workspaces = await _repository.GetByUserIdAsync(userId);

            // Mapear a DTO de listado, incluyendo el rol específico del usuario
            var response = workspaces
                .Select(ws =>
                {
                    var dto = _mapper.Map<WorkspaceListItemResponse>(ws);
                    // Determinar el rol del usuario solicitado (userId) en este espacio específico
                    var memberInfo = ws.Members.FirstOrDefault(m => m.UserId == userId);
                    dto.Role = memberInfo?.Role ?? string.Empty;
                    dto.JoinedAt = memberInfo?.JoinedAt ?? default;
                    return dto;
                })
                .ToList();

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WorkspaceResponse>> GetWorkspaceById(Guid id)
        {
            var workspace = await _repository.GetByIdAsync(id);

            if (workspace == null)
            {
                return NotFound();
            }

            // 1. Mapear modelo a WorkspaceResponse (tu DTO actualizado)
            var response = _mapper.Map<WorkspaceResponse>(workspace);

            // 2. Asignar campos que no pueden ser mapeados por AutoMapper
            response.Members = _mapper.Map<List<WorkspaceMemberResponse>>(workspace.Members);

            return Ok(response);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<WorkspaceResponse>> UpdateWorkspace(
            Guid id,
            [FromForm] UpdateWorkspaceRequest request
        )
        {
            var loggedInUserId = GetUserIdFromContext();
            var existingWorkspace = await _repository.GetByIdAsync(id);

            if (existingWorkspace == null)
                return NotFound();

            // 1. Autorización: Solo puede ser realizado por el propietario (o Admin)
            var userRole = existingWorkspace
                .Members.FirstOrDefault(m => m.UserId == loggedInUserId)
                ?.Role;
            if (userRole != "Propietario")
            {
                return Forbid();
            }

            // 2. Validar Nombre Único si se está modificando
            if (request.Name != null && await _repository.ExistsWithNameAsync(request.Name, id))
            {
                return BadRequest(new { Message = "El nuevo nombre del espacio ya está en uso." });
            }

            // 3. Manejo de Imagen (Si se proporciona una nueva, se reemplaza la anterior)
            if (request.ImageFile != null)
            {
                // Lógica de eliminación de imagen anterior
                if (!string.IsNullOrEmpty(existingWorkspace.ImagePublicId))
                {
                    // [MODIFICACIÓN 4: Implementación de la lógica de eliminación]
                    // Asumimos que la eliminación es fire-and-forget, no bloqueamos la actualización si falla la eliminación
                    await _photoService.DeletePhotoAsync(existingWorkspace.ImagePublicId);
                }

                // Subida de nueva imagen
                var uploadResult = await _photoService.AddPhotoAsync(request.ImageFile);
                if (uploadResult.Error != null)
                    return StatusCode(500, new { Message = "Error al subir la nueva imagen." });

                existingWorkspace.ImageUrl = uploadResult.SecureUrl.AbsoluteUri;
                existingWorkspace.ImagePublicId = uploadResult.PublicId; // Guardar el nuevo PublicId
            }

            // 4. Mapear y Actualizar Modelo
            _mapper.Map(request, existingWorkspace);
            existingWorkspace.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existingWorkspace);

            // 5. Retornar Respuesta
            var response = _mapper.Map<WorkspaceResponse>(existingWorkspace);
            response.Role = userRole;
            response.Members = _mapper.Map<List<WorkspaceMemberResponse>>(
                existingWorkspace.Members
            );

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkspace(Guid id)
        {
            var userId = GetAuthenticatedUserId();
            var workspace = await _repository.GetByIdAsync(id);

            if (workspace == null)
                return NotFound(); // Ya está inactivo o no existe

            if (workspace.OwnerId != userId)
            {
                return Forbid();
            }

            await _repository.SoftDeleteAsync(id);

            return NoContent(); // 204 No Content
        }
    }
}
