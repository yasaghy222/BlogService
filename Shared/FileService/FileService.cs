using BlogService.Models;
using BlogService.Shared;
using FluentValidation;
using FluentValidation.Results;

namespace FileService
{
    public class FileService(IValidator<AddFileDto> addValidator) : IFileService
    {
        private readonly IValidator<AddFileDto> _addValidator = addValidator;

        public async Task<Result> Add(AddFileDto model)
        {
            ValidationResult validationResult = _addValidator.Validate(model);
            if (!validationResult.IsValid)
                return CustomErrors.InvalidData(validationResult.Errors);

            try
            {
                long size = model.File.Length;

                if (model.File.Length <= 0)
                    return FileErrors.FileNotSelected();

                string path = $"Files/{model.Address}";
                Directory.CreateDirectory(path);

                string[] arr = model.File.FileName.Split(".");
                string ext = arr[^1];
                string? filePath = Path.Combine($"{path}/{model.Id}.{ext}");

                using var stream = File.Create(filePath);
                await model.File.CopyToAsync(stream);

                return FileResults.FileSaved($"{model.Address}/{model.Id}.{ext}");
            }
            catch (Exception)
            {
                return FileErrors.SaveFileFailed();
            }
        }

        public Result Delete(string address)
        {
            try
            {
                string path = Path.Combine($"Files/{address}");

                if (!File.Exists(path))
                    return FileErrors.FileNotFound();

                File.Delete(path);
                return FileResults.FileRemoved();
            }
            catch (Exception)
            {
                return FileErrors.RemoveFileFailed();
            }
        }
    }
}