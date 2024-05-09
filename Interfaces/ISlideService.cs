using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface ISlideService
{
	Task<Result> Add(AddSlideDto model);
	Task<Result> Edit(EditSlideDto model);
	Task<Result> ChangeStatus(Guid id, SlideStatus status);

	Task<Result> Delete(Guid id);
}
