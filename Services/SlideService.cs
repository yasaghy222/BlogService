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

public class SlideService(BlogServiceContext context,
					 IValidator<AddSlideDto> addValidator,
					 IValidator<EditSlideDto> editValidator) : ISlideService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly IValidator<AddSlideDto> _addValidator = addValidator;
	private readonly IValidator<EditSlideDto> _editValidator = editValidator;

	public async Task<Result> GetAllDetail(Guid sliderId, SlideStatus? status)
	{
		IQueryable<Slide> query = _context.Slides;

		if (status != null)
			query.Where(a => a.Status == status);

		List<Slide> slides = await query.Where(s => s.SliderId == sliderId)
															 .OrderBy(s => s.Order)
															 .ToListAsync();

		return CustomResults.SuccessOperation(slides.Adapt<List<SlideDetail>>());
	}

	public async Task<Result> Add(AddSlideDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Slide slide = model.Adapt<Slide>();
			await _context.Slides.AddAsync(slide);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(slide.Adapt<SlideDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}
	public async Task<Result> Edit(EditSlideDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Slide? oldData = await _context.Slides.SingleOrDefaultAsync(a => a.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			oldData = model.Adapt<Slide>();

			Slide slide = model.Adapt<Slide>();
			_context.Slides.Update(slide);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(slide.Adapt<SlideDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}



	public async Task<Result> ChangeStatus(Guid id, SlideStatus status)
	{
		Slide? oldData = await _context.Slides.SingleOrDefaultAsync(a => a.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Slides.Where(a => a.Id == id)
									 					 .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.Status, status));

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<SlideDetail>());
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
		Slide? oldData = await _context.Slides.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Slides.Where(a => a.Id == id).ExecuteDeleteAsync();
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
