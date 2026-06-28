using FluentValidation;

namespace Tickets.Application.Requests.Chats;

public sealed class UpdateChatRequestValidator : AbstractValidator<UpdateChatRequest>
{
    public UpdateChatRequestValidator()
    {
        RuleFor(x => x.Message)
            .NotEmpty()
            .MaximumLength(4000);
    }
}


