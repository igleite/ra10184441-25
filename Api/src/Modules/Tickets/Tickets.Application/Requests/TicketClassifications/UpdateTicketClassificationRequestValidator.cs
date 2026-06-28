using FluentValidation;

namespace Tickets.Application.Requests.TicketClassifications;

public sealed class UpdateTicketClassificationRequestValidator : AbstractValidator<UpdateTicketClassificationRequest>
{
    public UpdateTicketClassificationRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.RowVersion)
            .NotEmpty();
    }
}
