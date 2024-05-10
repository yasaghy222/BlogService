using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class AddSlideValidation : AbstractValidator<AddSlideDto>
{
	public AddSlideValidation()
	{
		RuleFor(p => p.SliderId).NotEmpty()
							.NotNull();

		RuleFor(p => p.Title).MaximumLength(100);

		RuleFor(p => p.Description).MaximumLength(200);

		RuleFor(p => p.Link).Matches(@"/(https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?\/[a-zA-Z0-9]{2,}|((https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z]{2,}(\.[a-zA-Z]{2,})(\.[a-zA-Z]{2,})?)|(https:\/\/www\.|http:\/\/www\.|https:\/\/|http:\/\/)?[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}\.[a-zA-Z0-9]{2,}(\.[a-zA-Z0-9]{2,})?/g;")
		.WithMessage("لینک وارد شده صحیح نمی باشد!");
	}
}
