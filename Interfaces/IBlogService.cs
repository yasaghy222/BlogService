using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface IBlogService
{
	Task<Result> GetInfo(Guid id);
	Task<Result> GetDetail(Guid id);
	Task<Result> GetAllInfo(int pageIndex, int pageSize);
	Task<Result> GetAllDetail(int pageIndex, int pageSize, BlogStatus? status);

	Task<Result> Add(AddBlogDto model);

	Task<Result> Edit(EditBlogDto model);
	Task<Result> ChangeStatus(Guid id, BlogStatus status, string statusDescription);

	Task<Result> Delete(Guid id);
}
