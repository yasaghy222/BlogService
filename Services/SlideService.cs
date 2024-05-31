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
using FileService;

namespace BlogService.Services;

public class SlideService(BlogServiceContext context,
					 IValidator<AddSlideDto> addValidator,
					 IValidator<EditSlideDto> editValidator,
					 IValidator<AddFileDto> fileValidator) : ISlideService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly FileService.FileService _fileService = new(fileValidator);
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

		Slide slide = model.Adapt<Slide>();

		Result fileResult = await _fileService.Add(new(slide.Id, model.Image, "Slide"));
		if (!fileResult.Status)
			return fileResult;

		slide.ImagePath = fileResult.Data?.ToString() ?? "";

		try
		{
			await _context.Slides.AddAsync(slide);
			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(slide.Adapt<SlideDetail>());
		}
		catch (Exception e)
		{
			_fileService.Delete(slide.ImagePath);
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

		string oldPath = oldData.ImagePath;

		_context.Entry(oldData).State = EntityState.Detached;
		oldData = model.Adapt<Slide>();
		oldData.ImagePath = oldPath;

		if (model.Image != null)
		{
			Result fileResult = await _fileService.Add(new(oldData.Id, model.Image, "Slide"));
			if (!fileResult.Status)
				return fileResult;

			oldData.ImagePath = fileResult.Data?.ToString() ?? "";
		}

		try
		{
			_context.Slides.Update(oldData);
			await _context.SaveChangesAsync();

			if (model.Image != null)
			{
				Result fileResult = _fileService.Delete(oldPath);
				if (!fileResult.Status)
					return fileResult;
			}

			return CustomResults.SuccessUpdate(oldData.Adapt<SlideDetail>());
		}
		catch (Exception e)
		{
			if (model.Image != null)
				_fileService.Delete(oldData.ImagePath);

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
