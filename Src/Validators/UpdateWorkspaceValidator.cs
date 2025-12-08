using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InsightFlow.WorkspacesService.Src.Validators
{
    public class UpdateWorkspaceValidator : AbstractValidator<UpdateWorkspaceRequest>
    {
        // private readonly IWorkspaceRepository _workspaceRepository;
        public UpdateWorkspaceValidator()
        {
            When(
                x => x.Name != null,
                () =>
                {
                    RuleFor(x => x.Name!)
                        .NotEmpty()
                        .Must(BeUniqueName)
                        .WithMessage("El nombre del espacio de trabajo no puede estar vacío.")
                        .MaximumLength(100)
                        .WithMessage(
                            "El nombre del espacio de trabajo no puede exceder los 100 caracteres."
                        );
                }
            );

            When(
                x => x.Description != null,
                () =>
                {
                    RuleFor(x => x.Description!)
                        .NotEmpty()
                        .WithMessage("La descripción del espacio de trabajo no puede estar vacía.")
                        .MaximumLength(500)
                        .WithMessage(
                            "La descripción del espacio de trabajo no puede exceder los 500 caracteres."
                        );
                }
            );

            When(
                x => x.Theme != null,
                () =>
                {
                    RuleFor(x => x.Theme!)
                        .NotEmpty()
                        .WithMessage("La temática del espacio de trabajo no puede estar vacía.")
                        .MaximumLength(50)
                        .WithMessage(
                            "La temática del espacio de trabajo no puede exceder los 50 caracteres."
                        );
                }
            );

            When(
                x => x.Image != null,
                () =>
                {
                    RuleFor(x => x.Image!)
                        .Must(file => file.Length > 0)
                        .WithMessage("La imagen del espacio de trabajo no puede estar vacía.");
                }
            );
        }

        private bool BeUniqueName(string name)
        {
            // Lógica para verificar si el nombre es único (por ejemplo, consultar la base de datos)
            return true; // Cambiar según la lógica real
        }
    }
}
