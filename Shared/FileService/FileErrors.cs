using BlogService.Models;

namespace FileService
{
    public class FileErrors
    {
        public static Result SaveFileFailed() => new()
        {
            Message = new()
            {
                Fa = "ذخیره فایل با مشکل مواجه شد",
                En = "Save File Failed!"
            },
            StatusCode = StatusCodes.Status500InternalServerError,
            Status = false
        };

        public static Result FileNotSelected() => new()
        {
            Message = new()
            {
                Fa = "انتخاب فایل اجباری می باشد",
                En = "Select File Is Required!"
            },
            StatusCode = StatusCodes.Status400BadRequest,
            Status = false
        };

        public static Result FileNotFound() => new()
        {
            Message = new()
            {
                Fa = "فایلی با مشخصات ارسالی یافت نشد",
                En = "File Not Found!"
            },
            StatusCode = StatusCodes.Status400BadRequest,
            Status = false
        };

        public static Result RemoveFileFailed() => new()
        {
            Message = new()
            {
                Fa = "حذف فایل با مشکل مواجه شد",
                En = "Remove File Failed!"
            },
            StatusCode = StatusCodes.Status500InternalServerError,
            Status = false
        };
    }
}