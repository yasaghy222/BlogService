using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class AddSliderValidation : AbstractValidator<AddSliderDto>
{
	public AddSliderValidation()
	{
		RuleFor(p => p.Key).MaximumLength(100);
		RuleFor(p => p.Description).MaximumLength(200);
	}
}
