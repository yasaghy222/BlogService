using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface ISliderService
{
	Task<Result> GetInfo(Guid id, string key);
	Task<Result> GetDetail(Guid id, string key);

	Task<Result> Add(AddSliderDto model);
	Task<Result> Edit(EditSliderDto model);
	Task<Result> ChangeStatus(Guid id, SliderStatus status);

	Task<Result> Delete(Guid id);
}
