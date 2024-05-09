using BlogService.DTOs;
using BlogService.Enums;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface ICategoryService
{
	Task<Result> Get(Guid id);
	Task<Result> GetAll(Guid? parentId);

	Task<Result> Add(AddCategoryDto model);
	Task<Result> Edit(EditCategoryDto model);

	Task<Result> Delete(Guid id);
}
