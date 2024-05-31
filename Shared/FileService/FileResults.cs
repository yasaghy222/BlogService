using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Models;

namespace FileService
{
    public class FileResults
    {
        public static Result FileSaved(string path) => new()
        {
            Message = new()
            {
                Fa = "ذخیره فایل با موفقیت انجام شد",
                En = "File Saved"
            },
            StatusCode = StatusCodes.Status201Created,
            Data = path
        };

        public static Result FileRemoved() => new()
        {
            Message = new()
            {
                Fa = "حذف فایل با موفقیت انجام شد",
                En = "File Removed"
            },
            StatusCode = StatusCodes.Status200OK,
        };
    }
}