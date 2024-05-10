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

public class SliderService(BlogServiceContext context,
					 IValidator<AddSliderDto> addValidator,
					 IValidator<EditSliderDto> editValidator) : ISliderService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly IValidator<AddSliderDto> _addValidator = addValidator;
	private readonly IValidator<EditSliderDto> _editValidator = editValidator;

	public async Task<Result> Add(AddSliderDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Slider Slider = model.Adapt<Slider>();
			await _context.Sliders.AddAsync(Slider);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(Slider.Adapt<SliderDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditSliderDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Slider? oldData = await _context.Sliders.SingleOrDefaultAsync(a => a.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			oldData = model.Adapt<Slider>();

			Slider Slider = model.Adapt<Slider>();
			_context.Sliders.Update(Slider);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(Slider.Adapt<SliderDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> GetInfo(Guid id, string key)
	{
		if (id == Guid.Empty)
			return CustomErrors.InvalidData("Id not Assigned!");

		Expression<Func<Slider, bool>> predicate = key.IsNullOrEmpty() ?
			s => s.Id == id : s => s.Id == id && s.Key.Contains(key);

		Slider? Slider = await _context.Sliders.SingleOrDefaultAsync(predicate);

		if (Slider == null)
			return CustomErrors.NotFoundData();
		else
			return CustomResults.SuccessOperation(Slider.Adapt<SliderInfo>());
	}


	public async Task<Result> GetDetail(Guid id, string key)
	{
		if (id == Guid.Empty)
			return CustomErrors.InvalidData("Id not Assigned!");

		Expression<Func<Slider, bool>> predicate = key.IsNullOrEmpty() ?
			s => s.Id == id : s => s.Id == id && s.Key.Contains(key);

		Slider? Slider = await _context.Sliders.SingleOrDefaultAsync(predicate);

		if (Slider == null)
			return CustomErrors.NotFoundData();
		else
			return CustomResults.SuccessOperation(Slider.Adapt<SliderDetail>());
	}

	public async Task<Result> GetAllInfo(string key)
	{
		IQueryable<Slider> query = _context.Sliders.Where(s => s.Status == SliderStatus.Visible);

		if (!key.IsNullOrEmpty())
			query.Where(s => s.Key.Contains(key));

		List<Slider> Sliders = await query.ToListAsync();

		return CustomResults.SuccessOperation(Sliders.Adapt<List<SliderInfo>>());
	}

	public async Task<Result> GetAllDetail(string key, SliderStatus? status)
	{
		IQueryable<Slider> query = _context.Sliders;

		if (!key.IsNullOrEmpty())
			query.Where(a => a.Key.Contains(key));

		if (status != null)
			query.Where(a => a.Status == status);

		List<Slider> Sliders = await query.ToListAsync();

		return CustomResults.SuccessOperation(Sliders.Adapt<List<SliderDetail>>());
	}

	public async Task<Result> ChangeStatus(Guid id, SliderStatus status)
	{
		Slider? oldData = await _context.Sliders.SingleOrDefaultAsync(a => a.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Sliders.Where(a => a.Id == id)
									 					 .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.Status, status));

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<SliderDetail>());
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
		Slider? oldData = await _context.Sliders.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Sliders.Where(a => a.Id == id).ExecuteDeleteAsync();
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
