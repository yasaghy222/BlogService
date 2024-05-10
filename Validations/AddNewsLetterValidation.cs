using BlogService.DTOs;
using FluentValidation;

namespace BlogService.Validations;

public class AddNewsLetterValidation : AbstractValidator<AddNewsLetterDto>
{
	public AddNewsLetterValidation()
	{
		RuleFor(nl => nl.Phone).NotEmpty()
								 .NotNull()
								 .Matches(@"^(\+98|0)?9\d{9}$")
								 .WithMessage("شماره وارد شده معتبر نمی باشد!");


		RuleFor(nl => nl.Email).EmailAddress()
											  .WithMessage("آدرس ایمیل وارد شده معتبر نمی باشد!");

	}
}
