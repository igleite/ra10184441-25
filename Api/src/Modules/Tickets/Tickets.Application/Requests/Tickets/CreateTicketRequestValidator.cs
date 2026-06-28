using FluentValidation;

namespace Tickets.Application.Requests.Tickets;

public sealed class CreateTicketRequestValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketRequestValidator()
    {
        RuleFor(x => x.CustomerId).NotEmpty();
        RuleFor(x => x.ArtifactId).NotEmpty();
        RuleFor(x => x.ClassificationId).NotEmpty();
        RuleFor(x => x.CreatedByUserId).NotEmpty();

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(4000);
    }
}


