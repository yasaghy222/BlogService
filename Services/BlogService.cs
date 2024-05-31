using BlogService.Data;
using BlogService.DTOs;
using BlogService.Models;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BlogService.Interfaces;
using BlogService.Shared;
using BlogService.Entities;
using BlogService.Enums;
using System.Linq.Expressions;
using FileService;

namespace BlogService.Services;

public class BlogService(BlogServiceContext context,
					 IValidator<AddBlogDto> addValidator,
					 IValidator<EditBlogDto> editValidator,
					 IValidator<AddFileDto> fileValidator) : IBlogService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly FileService.FileService _fileService = new(fileValidator);
	private readonly IValidator<AddBlogDto> _addValidator = addValidator;
	private readonly IValidator<EditBlogDto> _editValidator = editValidator;

	public async Task<Result> Add(AddBlogDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Category? category = await _context.Categories.SingleOrDefaultAsync(s => s.Id == model.CategoryId);
		if (category == null)
			return CustomErrors.NotFoundData("دستبندی انتخاب شده یافت نشد!");

		Blog blog = model.Adapt<Blog>();

		Result fileResult = await _fileService.Add(new(blog.Id, model.Image, "Blog"));
		if (!fileResult.Status)
			return fileResult;

		blog.ImagePath = fileResult.Data?.ToString() ?? "";

		try
		{
			await _context.Blogs.AddAsync(blog);
			await _context.SaveChangesAsync();

			blog.Category = category;
			return CustomResults.SuccessCreation(blog.Adapt<BlogDetail>());
		}
		catch (Exception e)
		{
			_fileService.Delete(blog.ImagePath);
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditBlogDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Blog? oldData = await _context.Blogs.SingleOrDefaultAsync(c => c.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		string oldPath = oldData.ImagePath;

		_context.Entry(oldData).State = EntityState.Detached;
		oldData = model.Adapt<Blog>();
		oldData.ImagePath = oldPath;

		if (model.Image != null)
		{
			Result fileResult = await _fileService.Add(new(oldData.Id, model.Image, "Blog"));
			if (!fileResult.Status)
				return fileResult;

			oldData.ImagePath = fileResult.Data?.ToString() ?? "";
		}

		try
		{
			_context.Blogs.Update(oldData);
			await _context.SaveChangesAsync();

			if (model.Image != null)
			{
				Result fileResult = _fileService.Delete(oldPath);
				if (!fileResult.Status)
					return fileResult;
			}

			return CustomResults.SuccessUpdate(oldData.Adapt<BlogDetail>());
		}
		catch (Exception e)
		{
			if (model.Image != null)
				_fileService.Delete(oldData.ImagePath);

			return CustomErrors.InternalServer(e.Message);
		}
	}

	private async Task<Result> Get(Guid id, Expression<Func<Blog, bool>> predicate)
	{
		if (id == Guid.Empty)
			return CustomErrors.InvalidData("Id not Assigned!");

		Blog? blog = await _context.Blogs.SingleOrDefaultAsync(predicate);
		if (blog == null)
			return CustomErrors.NotFoundData();

		return CustomResults.SuccessOperation(blog);
	}

	public async Task<Result> GetInfo(Guid id)
	{
		Result result = await Get(id, b => b.Id == id && b.Status == BlogStatus.Confirmed);

		if (result.Status)
			return CustomResults.SuccessOperation(result.Data.Adapt<BlogInfo>());
		else
			return result;
	}
	public async Task<Result> GetDetail(Guid id)
	{
		Result result = await Get(id, b => b.Id == id);

		if (result.Status)
			return CustomResults.SuccessOperation(result.Data.Adapt<BlogDetail>());
		else
			return result;
	}

	public async Task<Result> GetAllInfo(int pageIndex = 1, int pageSize = 10)
	{
		pageIndex = pageIndex < 1 ? 1 : pageIndex;

		List<Blog> blogs = await _context.Blogs.Where(b => b.Status == BlogStatus.Confirmed)
																			.OrderByDescending(b => b.PublishedDate)
																			.Skip((pageIndex - 1) * pageSize)
																			.Take(pageSize)
																			.ToListAsync();

		return CustomResults.SuccessOperation(blogs.Adapt<List<BlogInfo>>());
	}

	public async Task<Result> GetAllDetail(int pageIndex = 1, int pageSize = 10, BlogStatus? status = null)
	{
		pageIndex = pageIndex < 1 ? 1 : pageIndex;

		IQueryable<Blog> query = _context.Blogs;
		if (status != null)
			query.Where(b => b.Status == status);

		List<Blog> blogs = await query.OrderByDescending(b => b.PublishedDate)
															.Skip((pageIndex - 1) * pageSize)
															.Take(pageSize)
															.ToListAsync();

		return CustomResults.SuccessOperation(blogs.Adapt<List<BlogDetail>>());
	}

	public async Task<Result> ChangeStatus(Guid id, BlogStatus status, string? statusDescription)
	{
		Blog? oldData = await _context.Blogs.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Blogs.Where(b => b.Id == id)
									 					 .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Status, status)
														 																	 .SetProperty(b => b.StatusDescription, statusDescription));

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<BlogDetail>());
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
		Blog? oldData = await _context.Blogs.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Blogs.Where(b => b.Id == id).ExecuteDeleteAsync();
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
