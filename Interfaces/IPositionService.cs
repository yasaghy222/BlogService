using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface IPositionService
{
	Task<Result> Get(Guid id);
	Task<Result> Get(string key);
	Task<Result> GetAll(PositionType? type);
	Task<Result> GetAll(int pageIndex, int pageSize, PositionType? type);

	Task<Result> Add(AddPositionDto model);
	Task<Result> Edit(EditPositionDto model);

	Task<Result> Delete(Guid id);
}
