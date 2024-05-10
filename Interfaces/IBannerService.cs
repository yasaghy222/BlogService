using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface IBannerService
{
	Task<Result> GetInfo(Guid id);
	Task<Result> GetDetail(Guid id);
	Task<Result> GetAllInfo();
	Task<Result> GetAllDetail(int pageIndex, int pageSize, BannerStatus? status);


	Task<Result> Add(AddBannerDto model);
	Task<Result> Edit(EditBannerDto model);
	Task<Result> ChangeStatus(Guid id, BannerStatus status);

	Task<Result> Delete(Guid id);
}
