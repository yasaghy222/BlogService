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
using FileService;

namespace BlogService.Services;

public class AuthorService(BlogServiceContext context,
					 IValidator<AddAuthorDto> addValidator,
					 IValidator<EditAuthorDto> editValidator,
					 IValidator<AddFileDto> fileValidator) : IAuthorService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly FileService.FileService _fileService = new(fileValidator);
	private readonly IValidator<AddAuthorDto> _addValidator = addValidator;
	private readonly IValidator<EditAuthorDto> _editValidator = editValidator;

	public async Task<Result> Add(AddAuthorDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Author author = model.Adapt<Author>();

		Result fileResult = await _fileService.Add(new(author.Id, model.Image, "Author"));
		if (!fileResult.Status)
			return fileResult;

		try
		{
			await _context.Authors.AddAsync(author);
			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(author.Adapt<AuthorDetail>());
		}
		catch (Exception e)
		{
			_fileService.Delete(author.ImagePath);
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

		string oldPath = oldData.ImagePath;

		_context.Entry(oldData).State = EntityState.Detached;
		oldData = model.Adapt<Author>();
		oldData.ImagePath = oldPath;

		if (model.Image != null)
		{
			Result fileResult = await _fileService.Add(new(oldData.Id, model.Image, "Author"));
			if (!fileResult.Status)
				return fileResult;

			oldData.ImagePath = fileResult.Data?.ToString() ?? "";
		}

		try
		{
			_context.Authors.Update(oldData);
			await _context.SaveChangesAsync();

			if (model.Image != null)
			{
				Result fileResult = _fileService.Delete(oldPath);
				if (!fileResult.Status)
					return fileResult;
			}

			return CustomResults.SuccessUpdate(oldData.Adapt<AuthorDetail>());
		}
		catch (Exception e)
		{
			if (model.Image != null)
				_fileService.Delete(oldData.ImagePath);

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

	public async Task<Result> GetAllDetail(int pageIndex = 1, int pageSize = 10, AuthorStatus? status = null)
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
			{
				Result fileResult = await _fileService.Delete(oldData.ImagePath);
				if (!fileResult.Status)
					return fileResult;

				return CustomResults.SuccessDelete();
			}
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
