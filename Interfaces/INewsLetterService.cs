using BlogService.DTOs;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface INewsLetterService
{
	Task<Result> GetAll(int pageIndex = 1, int pageSize = 10, string phone = "", string email = "");

	Task<Result> Add(AddNewsLetterDto model);

	Task<Result> Delete(Guid id);
}
