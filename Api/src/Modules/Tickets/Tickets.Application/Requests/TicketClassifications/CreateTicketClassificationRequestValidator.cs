using FluentValidation;

namespace Tickets.Application.Requests.TicketClassifications;

public sealed class CreateTicketClassificationRequestValidator : AbstractValidator<CreateTicketClassificationRequest>
{
    public CreateTicketClassificationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);
    }
}
