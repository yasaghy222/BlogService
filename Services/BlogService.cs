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

namespace BlogService.Services;

public class BlogService(BlogServiceContext context,
					 IValidator<AddBlogDto> addValidator,
					 IValidator<EditBlogDto> editValidator) : IBlogService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly IValidator<AddBlogDto> _addValidator = addValidator;
	private readonly IValidator<EditBlogDto> _editValidator = editValidator;

	public async Task<Result> Add(AddBlogDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Blog blog = model.Adapt<Blog>();
			await _context.Blogs.AddAsync(blog);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(blog.Adapt<BlogDetail>());
		}
		catch (Exception e)
		{
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

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			Blog blog = model.Adapt<Blog>();
			_context.Blogs.Update(blog);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(blog.Adapt<BlogDetail>());
		}
		catch (Exception e)
		{
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
		List<Blog> blogs = await _context.Blogs.Where(b => b.Status == BlogStatus.Confirmed)
																			.OrderByDescending(b => b.PublishedDate)
																			.Skip((pageIndex - 1) * pageSize)
																			.Take(pageSize)
																			.ToListAsync();

		return CustomResults.SuccessOperation(blogs.Adapt<List<BlogInfo>>());
	}

	public async Task<Result> GetAllDetail(int pageIndex, int pageSize, BlogStatus? status)
	{

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
