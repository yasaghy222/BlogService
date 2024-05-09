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

public class BannerService(BlogServiceContext context,
					 IValidator<AddBannerDto> addValidator,
					 IValidator<EditBannerDto> editValidator) : IBannerService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly IValidator<AddBannerDto> _addValidator = addValidator;
	private readonly IValidator<EditBannerDto> _editValidator = editValidator;

	public async Task<Result> Add(AddBannerDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Banner banner = model.Adapt<Banner>();
			await _context.Banners.AddAsync(banner);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(banner.Adapt<BannerDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditBannerDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Banner? oldData = await _context.Banners.SingleOrDefaultAsync(i => i.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			Banner banner = model.Adapt<Banner>();
			_context.Banners.Update(banner);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(banner.Adapt<BannerDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> ChangeStatus(Guid id, BannerStatus status)
	{
		Banner? oldData = await _context.Banners.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Banners.Where(b => b.Id == id)
									 					 .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.Status, status));

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<BannerDetail>());
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
		Banner? oldData = await _context.Banners.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Banners.Where(b => b.Id == id).ExecuteDeleteAsync();
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
