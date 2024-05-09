using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class AddCategoryValidation : AbstractValidator<AddCategoryDto>
{
	public AddCategoryValidation()
	{
		RuleFor(b => b.Title).NotEmpty()
							.NotNull()
							.MaximumLength(100);

		RuleFor(b => b.EnTitle).NotEmpty()
								.NotNull()
								.MaximumLength(100);

		RuleFor(b => b.Description)
							.MaximumLength(500);
	}
}
