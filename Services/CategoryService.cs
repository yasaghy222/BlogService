using BlogService.Models;
using FluentValidation;
using FluentValidation.Results;
using Mapster;
using Microsoft.EntityFrameworkCore;
using BlogService.Interfaces;
using BlogService.Shared;
using BlogService.Entities;
using BlogService.Data;
using BlogService.DTOs;

namespace BlogService.Services;

public class CategoryService(BlogServiceContext context,
					 IValidator<AddCategoryDto> addValidator,
					 IValidator<EditCategoryDto> editValidator) : ICategoryService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly IValidator<AddCategoryDto> _addValidator = addValidator;
	private readonly IValidator<EditCategoryDto> _editValidator = editValidator;

	public async Task<Result> Add(AddCategoryDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Category category = model.Adapt<Category>();
			await _context.Categories.AddAsync(category);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(category.Adapt<CategoryDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditCategoryDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Category? oldData = await _context.Categories.SingleOrDefaultAsync(a => a.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			Category category = model.Adapt<Category>();
			_context.Categories.Update(category);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(category.Adapt<CategoryDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Get(Guid id)
	{
		if (id == Guid.Empty)
			return CustomErrors.InvalidData("Id not Assigned!");

		Category? category = await _context.Categories.Include(c => c.Parent)
																		 .Include(c => c.Blogs)
																		.SingleOrDefaultAsync(c => c.Id == id);
		if (category == null)
			return CustomErrors.NotFoundData();
		else
			return CustomResults.SuccessOperation(category.Adapt<CategoryDetail>());
	}

	public async Task<Result> GetAll(Guid? parentId = null)
	{
		List<Category> categories = await _context.Categories
																			.Include(c => c.Chields)
																			.Where(c => c.ParentId == parentId)
																			.ToListAsync();

		return CustomResults.SuccessOperation(categories.Adapt<List<CategoryInfo>>());
	}

	public async Task<Result> Delete(Guid id)
	{
		Category? oldData = await _context.Categories.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		if (oldData.Blogs?.Count > 0)
			return CustomErrors.NotAcceptable("به ازای این دسته تعدادی پست ثبت شده است به همین دلیل امکان حذف این دسته وجود ندارد!");

		try
		{
			int effectedRowCount = await _context.Categories.Where(a => a.Id == id).ExecuteDeleteAsync();
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
