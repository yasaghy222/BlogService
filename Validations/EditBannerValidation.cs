using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class EditBannerValidation : AbstractValidator<EditBannerDto>
{
	public EditBannerValidation()
	{
		RuleFor(b => b.Id).NotEmpty()
										  .NotNull();

		RuleFor(b => b.Title)
							.MaximumLength(100);

		RuleFor(b => b.Description)
							.MaximumLength(500);
	}
}
