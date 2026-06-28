using FluentValidation;

namespace Tickets.Application.Requests.StatusReasons;

public sealed class CreateStatusReasonRequestValidator : AbstractValidator<CreateStatusReasonRequest>
{
    public CreateStatusReasonRequestValidator()
    {
        RuleFor(x => x.Type)
            .Must(type => type is 1 or 2)
            .WithMessage("Tipo inválido.");

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}


