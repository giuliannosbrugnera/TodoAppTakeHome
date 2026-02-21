using FluentValidation;
using TodoAppTakeHome.Api.DTOs;

namespace TodoAppTakeHome.Api.Validators;

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(x => x.Title).MaximumLength(200).When(x => x.Title != null);
    }
}
