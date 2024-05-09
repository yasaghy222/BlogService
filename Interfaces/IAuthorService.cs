using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface IAuthorService
{
	Task<Result> GetInfo(Guid id);
	Task<Result> GetDetail(Guid id);
	Task<Result> GetAllInfo(int pageIndex, int pageSize);
	Task<Result> GetAllDetail(int pageIndex, int pageSize, AuthorStatus? status);

	Task<Result> Add(AddAuthorDto model);

	Task<Result> Edit(EditAuthorDto model);
	Task<Result> ChangeStatus(Guid id, AuthorStatus status, string statusDescription);

	Task<Result> Delete(Guid id);
}
