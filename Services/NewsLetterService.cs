using BlogService.Models;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BlogService.Interfaces;
using BlogService.Shared;
using BlogService.Entities;
using BlogService.Enums;
using BlogService.Data;
using BlogService.DTOs;
using System.Linq.Expressions;
using Microsoft.IdentityModel.Tokens;

namespace BlogService.Services;

public class NewsLetterService(BlogServiceContext context,
					 IValidator<AddNewsLetterDto> addValidator) : INewsLetterService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly IValidator<AddNewsLetterDto> _addValidator = addValidator;

	public async Task<Result> GetAll(int pageIndex = 1, int pageSize = 10, string phone = "", string email = "")
	{
		IQueryable<NewsLetter> query = _context.NewsLetters;

		if (!phone.IsNullOrEmpty())
			query.Where(a => a.Phone.Contains(phone));

		if (!email.IsNullOrEmpty())
			query.Where(a => a.Email.Contains(email));

		pageIndex = pageIndex < 1 ? 1 : pageIndex;

		List<NewsLetter> NewsLetters = await query.Skip((pageIndex - 1) * pageSize)
																		.Take(pageSize)
																		 .ToListAsync();

		return CustomResults.SuccessOperation(NewsLetters.Adapt<List<NewsLetterDetail>>());
	}

	public async Task<Result> Add(AddNewsLetterDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			NewsLetter NewsLetter = model.Adapt<NewsLetter>();
			await _context.NewsLetters.AddAsync(NewsLetter);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(NewsLetter.Adapt<NewsLetterDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Delete(Guid id)
	{
		NewsLetter? oldData = await _context.NewsLetters.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.NewsLetters.Where(a => a.Id == id).ExecuteDeleteAsync();
			if (effectedRowCount == 1)
				return CustomResults.SuccessDelete();
			else
				return CustomErrors.NotFoundData();
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public void Dispose()
	{
		_context.Dispose();
	}
}
