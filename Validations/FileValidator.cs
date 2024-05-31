using FluentValidation;

namespace BlogService.Validations
{
    public class FileValidator<T> : AbstractValidator<IFormFile?>
    {
        public FileValidator(IFormFile? file)
        {
            if (file == null)
                return;

            if (typeof(T).ToString().Contains("Add"))
                RuleFor(x => x).NotEmpty().NotNull().WithMessage("فایل انتخاب نشده است!");

            RuleFor(x => x.Length).LessThanOrEqualTo(100000)
                    .WithMessage("حجم فایل انتخابی بیش از یک مگابایت است!");

            RuleFor(x => x.ContentType).Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                .WithMessage("فرمت فایل انتخابی از نوع تصویر نیست!");
        }
    }
}