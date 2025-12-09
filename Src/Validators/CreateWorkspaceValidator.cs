using FluentValidation;
using InsightFlow.WorkspacesService.Src.DTOs;

namespace InsightFlow.WorkspacesService.Src.Validators
{
    public class CreateWorkspaceValidator : AbstractValidator<CreateWorkspaceRequest>
    {
        public CreateWorkspaceValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("El nombre del espacio de trabajo es obligatorio.")
                .MaximumLength(100)
                .WithMessage(
                    "El nombre del espacio de trabajo no puede exceder los 100 caracteres."
                );

            RuleFor(x => x.Description)
                .NotEmpty()
                .WithMessage("La descripción del espacio de trabajo es obligatoria.")
                .MaximumLength(500)
                .WithMessage(
                    "La descripción del espacio de trabajo no puede exceder los 500 caracteres."
                );

            RuleFor(x => x.Theme)
                .NotEmpty()
                .WithMessage("La temática del espacio de trabajo es obligatoria.")
                .MaximumLength(50)
                .WithMessage(
                    "La temática del espacio de trabajo no puede exceder los 50 caracteres."
                );

            RuleFor(x => x.Image)
                .NotNull()
                .WithMessage("La imagen del espacio de trabajo es obligatoria.")
                .Must(file => file.Length > 0)
                .WithMessage("La imagen del espacio de trabajo no puede estar vacía.");
        }
    }
}
