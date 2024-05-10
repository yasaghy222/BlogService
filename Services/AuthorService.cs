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

namespace BlogService.Services;

public class AuthorService(BlogServiceContext context,
					 IValidator<AddAuthorDto> addValidator,
					 IValidator<EditAuthorDto> editValidator) : IAuthorService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly IValidator<AddAuthorDto> _addValidator = addValidator;
	private readonly IValidator<EditAuthorDto> _editValidator = editValidator;

	public async Task<Result> Add(AddAuthorDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Author author = model.Adapt<Author>();
			await _context.Authors.AddAsync(author);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(author.Adapt<AuthorDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditAuthorDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Author? oldData = await _context.Authors.SingleOrDefaultAsync(a => a.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			oldData = model.Adapt<Author>();

			Author author = model.Adapt<Author>();
			_context.Authors.Update(author);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(author.Adapt<AuthorDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	private async Task<Result> Get(Guid id, Expression<Func<Author, bool>> predicate)
	{
		if (id == Guid.Empty)
			return CustomErrors.InvalidData("Id not Assigned!");

		Author? author = await _context.Authors.SingleOrDefaultAsync(predicate);
		if (author == null)
			return CustomErrors.NotFoundData();

		return CustomResults.SuccessOperation(author);
	}

	public async Task<Result> GetInfo(Guid id)
	{
		Result result = await Get(id, a => a.Id == id & a.Status == AuthorStatus.Active);

		if (result.Status)
			return CustomResults.SuccessOperation(result.Data.Adapt<AuthorInfo>());
		else
			return result;
	}
	public async Task<Result> GetDetail(Guid id)
	{
		Result result = await Get(id, a => a.Id == id);

		if (result.Status)
			return CustomResults.SuccessOperation(result.Data.Adapt<AuthorDetail>());
		else
			return result;
	}

	public async Task<Result> GetAllInfo(int pageIndex = 1, int pageSize = 10)
	{
		List<Author> authors = await _context.Authors.Where(a => a.Status == AuthorStatus.Active)
																			.Skip((pageIndex - 1) * pageSize)
																			.Take(pageSize)
																			.ToListAsync();

		return CustomResults.SuccessOperation(authors.Adapt<List<AuthorInfo>>());
	}

	public async Task<Result> GetAllDetail(int pageIndex, int pageSize, AuthorStatus? status)
	{
		IQueryable<Author> query = _context.Authors;
		if (status != null)
			query.Where(a => a.Status == status);

		List<Author> authors = await query.Skip((pageIndex - 1) * pageSize)
																   .Take(pageSize)
																   .ToListAsync();

		return CustomResults.SuccessOperation(authors.Adapt<List<AuthorDetail>>());
	}

	public async Task<Result> ChangeStatus(Guid id, AuthorStatus status, string? statusDescription)
	{
		Author? oldData = await _context.Authors.SingleOrDefaultAsync(a => a.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Authors.Where(a => a.Id == id)
									 					 .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.Status, status)
														 																	 .SetProperty(a => a.StatusDescription, statusDescription));

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<AuthorDetail>());
			else
				return CustomErrors.NotFoundData();
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Delete(Guid id)
	{
		Author? oldData = await _context.Authors.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Authors.Where(a => a.Id == id).ExecuteDeleteAsync();
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
