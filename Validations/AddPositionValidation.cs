using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class AddPositionValidation : AbstractValidator<AddPositionDto>
{
	public AddPositionValidation()
	{
		RuleFor(p => p.Key).NotEmpty()
							.NotNull()
							.MaximumLength(100);

		RuleFor(p => p.ForeignId).NotEmpty()
							.NotNull();
	}
}
