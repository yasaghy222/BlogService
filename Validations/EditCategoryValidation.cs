using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class EditCategoryValidation : AbstractValidator<EditCategoryDto>
{
	public EditCategoryValidation()
	{
		RuleFor(b => b.Id).NotEmpty()
								.NotNull();

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
