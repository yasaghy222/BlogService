using BlogService.DTOs;
using BlogService.Models;

namespace BlogService.Interfaces;

public interface INewsLetterService
{
	Task<Result> GetAll(string phone, string email);
	Task<Result> GetAll(int pageIndex, int pageSize, string phone, string email);

	Task<Result> Add(AddNewsLetterDto model);

	Task<Result> Delete(Guid id);
}
