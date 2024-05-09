using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class AddBannerValidation : AbstractValidator<AddBannerDto>
{
	public AddBannerValidation()
	{
		RuleFor(b => b.Title)
							.MaximumLength(100);

		RuleFor(b => b.Description)
							.MaximumLength(500);
	}
}
