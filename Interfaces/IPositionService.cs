using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface IPositionService
{
	Task<Result> GetDetail(Guid id, string key);
	Task<Result> GetAllInfo(string key, PositionType? type);
	Task<Result> GetAllDetail(int pageIndex, int pageSize, string key, PositionType? type, PositionStatus? status);

	Task<Result> Add(AddPositionDto model);
	Task<Result> Edit(EditPositionDto model);
	Task<Result> ChangeStatus(Guid id, PositionStatus status);

	Task<Result> Delete(Guid id);
}
