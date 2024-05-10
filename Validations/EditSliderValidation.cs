using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class EditSliderValidation : AbstractValidator<EditSliderDto>
{
	public EditSliderValidation()
	{
		RuleFor(p => p.Id).NotEmpty().NotNull();
		RuleFor(p => p.Key).MaximumLength(100);
		RuleFor(p => p.Description).MaximumLength(200);
	}
}
