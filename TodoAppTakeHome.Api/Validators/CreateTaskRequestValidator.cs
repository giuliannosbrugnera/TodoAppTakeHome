using FluentValidation;
using TodoAppTakeHome.Api.DTOs;

namespace TodoAppTakeHome.Api.Validators;

public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
{
    public CreateTaskRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
    }
}
