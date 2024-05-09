using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class EditBlogValidation : AbstractValidator<EditBlogDto>
{
	public EditBlogValidation()
	{
		RuleFor(b => b.Id).NotEmpty()
								.NotNull();

		RuleFor(b => b.EnTitle).NotEmpty()
							.NotNull()
							.MaximumLength(100);

		RuleFor(b => b.Title).NotEmpty()
							.NotNull()
							.MaximumLength(100);

		RuleFor(b => b.Description).NotEmpty()
							.NotNull()
							.MaximumLength(500);


		RuleFor(b => b.Content).NotEmpty()
							.NotNull();


		RuleFor(b => b.Tags).NotEmpty()
							.NotNull();

		RuleFor(b => b.ResourceLinks).NotEmpty()
							.NotNull();
	}
}
