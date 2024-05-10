using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class EditPositionValidation : AbstractValidator<EditPositionDto>
{
	public EditPositionValidation()
	{
		RuleFor(p => p.Id).NotEmpty()
									.NotNull();

		RuleFor(p => p.Key).NotEmpty()
							.NotNull()
							.MaximumLength(100);

		RuleFor(p => p.ForeignId).NotEmpty()
							.NotNull();
	}
}
