using BlogService.Models;

namespace BlogService.Interfaces;

public interface ISliderSlideService
{
	Task<Result> GetAll(Guid sliderId);
	Task<Result> Add(Guid slideId, Guid sliderId);
	Task<Result> Delete(Guid id);
}
