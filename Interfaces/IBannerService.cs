using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface IBannerService
{
	Task<Result> Add(AddBannerDto model);

	Task<Result> Edit(EditBannerDto model);
	Task<Result> ChangeStatus(Guid id, BannerStatus status);

	Task<Result> Delete(Guid id);
}
