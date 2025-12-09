using AutoMapper;
using InsightFlow.WorkspacesService.Src.DTOs;
using InsightFlow.WorkspacesService.Src.DTOs.Response;
using InsightFlow.WorkspacesService.Src.Models;

namespace InsightFlow.WorkspacesService.Src.Profiles
{
    public class WorkspaceMappingProfile : Profile
    {
        public WorkspaceMappingProfile()
        {
            // --- Mapeo de Solicitud a Modelo (Para Creación) ---
            // Mapea CreateWorkspaceRequest a Workspace
            CreateMap<CreateWorkspaceRequest, Workspace>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // El Id se generará en el Repository/Service
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)) // Por defecto activo
                .ForMember(dest => dest.Members, opt => opt.Ignore()); // Los miembros se añaden después de la creación

            // --- Mapeo de Solicitud a Modelo (Para Actualización) ---
            // Mapea UpdateWorkspaceRequest a Workspace (se usa con la instancia existente del modelo)
            // Solo mapea los campos editables
            CreateMap<UpdateWorkspaceRequest, Workspace>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != null))
                .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => src.Image != null))
                // Ignora todos los demás campos que no deben ser modificados por este DTO
                .ForAllMembers(opt =>
                    opt.Condition(
                        (src, dest, srcMember) =>
                            (srcMember != null) || (dest.Name == null && dest.ImageUrl == null)
                    )
                );

            // --- Mapeo de Modelo a Respuesta (Para Visualización) ---
            // Mapea Workspace a WorkspaceResponse
            CreateMap<Workspace, WorkspaceResponse>();

            // Mapeo de miembros para asegurar que el DTO de respuesta se serialice correctamente
            // CreateMap<WorkspaceMember, WorkspaceMemberResponse>();
        }
    }
}
