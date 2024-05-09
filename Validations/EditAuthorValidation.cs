using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class EditAuthorValidation : AbstractValidator<EditAuthorDto>
{
	public EditAuthorValidation()
	{
		RuleFor(b => b.Id).NotEmpty()
								.NotNull();

		RuleFor(b => b.Title).NotEmpty()
							.NotNull()
							.MaximumLength(100);

		RuleFor(b => b.Description).NotEmpty()
							.NotNull()
							.MaximumLength(500);
	}
}
