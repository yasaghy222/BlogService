using BlogService.DTOs;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface ISliderService
{
	Task<Result> Get(Guid id);
	Task<Result> Get(string title);

	Task<Result> Add(AddSliderDto model);
	Task<Result> Edit(EditSliderDto model);

	Task<Result> Delete(Guid id);
}
