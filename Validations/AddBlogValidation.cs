using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class AddBlogValidation : AbstractValidator<AddBlogDto>
{
	public AddBlogValidation()
	{
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


		RuleFor(b => b.AuthorId).NotEmpty()
							.NotNull();
	}
}
