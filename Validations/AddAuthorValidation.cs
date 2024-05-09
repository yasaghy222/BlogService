using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class AddAuthorValidation : AbstractValidator<AddAuthorDto>
{
	public AddAuthorValidation()
	{
		RuleFor(b => b.Title).NotEmpty()
							.NotNull()
							.MaximumLength(100);

		RuleFor(b => b.Description).NotEmpty()
							.NotNull()
							.MaximumLength(500);
	}
}
