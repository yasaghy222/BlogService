using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogService.Models;
using FileService;

namespace FileService
{
    public interface IFileService
    {
        Task<Result> Add(AddFileDto model);
        Result Delete(string address);
    }
}