using FluentValidation;

namespace Tickets.Application.Requests.Chats;

public sealed class CreateChatRequestValidator : AbstractValidator<CreateChatRequest>
{
    public CreateChatRequestValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();

        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(4000);
    }
}


