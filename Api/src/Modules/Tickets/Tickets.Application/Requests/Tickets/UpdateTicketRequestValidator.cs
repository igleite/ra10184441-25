using FluentValidation;

namespace Tickets.Application.Requests.Tickets;

public sealed class UpdateTicketRequestValidator : AbstractValidator<UpdateTicketRequest>
{
    public UpdateTicketRequestValidator()
    {
        RuleFor(x => x.StatusId).NotEmpty();
        RuleFor(x => x.ClassificationId).NotEmpty();
        RuleFor(x => x.ArtifactId).NotEmpty();

        RuleFor(x => x.AllocationCenter)
            .InclusiveBetween(1, 2);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(4000);

        RuleFor(x => x.Resolution)
            .NotNull()
            .MaximumLength(4000);
    }
}


