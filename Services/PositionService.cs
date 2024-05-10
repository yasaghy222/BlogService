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

public class PositionService(BlogServiceContext context,
					 IValidator<AddPositionDto> addValidator,
					 IValidator<EditPositionDto> editValidator) : IPositionService, IDisposable
{
	private readonly BlogServiceContext _context = context;
	private readonly IValidator<AddPositionDto> _addValidator = addValidator;
	private readonly IValidator<EditPositionDto> _editValidator = editValidator;

	public async Task<Result> Add(AddPositionDto model)
	{
		ValidationResult validationResult = _addValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		try
		{
			Position position = model.Adapt<Position>();
			await _context.Positions.AddAsync(position);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessCreation(position.Adapt<PositionDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> Edit(EditPositionDto model)
	{
		ValidationResult validationResult = _editValidator.Validate(model);
		if (!validationResult.IsValid)
			return CustomErrors.InvalidData(validationResult.Errors);

		Position? oldData = await _context.Positions.SingleOrDefaultAsync(a => a.Id == model.Id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			_context.Entry(oldData).State = EntityState.Detached;

			oldData = model.Adapt<Position>();

			Position position = model.Adapt<Position>();
			_context.Positions.Update(position);

			await _context.SaveChangesAsync();

			return CustomResults.SuccessUpdate(position.Adapt<PositionDetail>());
		}
		catch (Exception e)
		{
			return CustomErrors.InternalServer(e.Message);
		}
	}

	public async Task<Result> GetDetail(Guid id, string key)
	{
		if (id == Guid.Empty)
			return CustomErrors.InvalidData("Id not Assigned!");

		Expression<Func<Position, bool>> predicate = key.IsNullOrEmpty() ?
			p => p.Id == id : p => p.Id == id && p.Key.Contains(key);

		Position? position = await _context.Positions.SingleOrDefaultAsync(predicate);

		if (position == null)
			return CustomErrors.NotFoundData();
		else
			return CustomResults.SuccessOperation(position.Adapt<PositionDetail>());
	}

	public async Task<Result> GetAllInfo(string key, PositionType? type)
	{
		IQueryable<Position> query = _context.Positions.Where(p => p.Status == PositionStatus.Visible);

		if (!key.IsNullOrEmpty())
			query.Where(p => p.Key.Contains(key));

		if (type != null)
			query.Where(p => p.Type == type);

		List<Position> positions = await query.ToListAsync();

		return CustomResults.SuccessOperation(positions.Adapt<List<PositionInfo>>());
	}

	public async Task<Result> GetAllDetail(int pageIndex, int pageSize, string key, PositionType? type, PositionStatus? status)
	{
		IQueryable<Position> query = _context.Positions;

		if (!key.IsNullOrEmpty())
			query.Where(a => a.Key.Contains(key));

		if (type != null)
			query.Where(p => p.Type == type);

		if (status != null)
			query.Where(a => a.Status == status);

		List<Position> positions = await query.Skip((pageIndex - 1) * pageSize)
																   .Take(pageSize)
																   .ToListAsync();

		return CustomResults.SuccessOperation(positions.Adapt<List<PositionDetail>>());
	}

	public async Task<Result> ChangeStatus(Guid id, PositionStatus status)
	{
		Position? oldData = await _context.Positions.SingleOrDefaultAsync(a => a.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Positions.Where(a => a.Id == id)
									 					 .ExecuteUpdateAsync(setters => setters.SetProperty(a => a.Status, status));

			if (effectedRowCount == 1)
				return CustomResults.SuccessUpdate(oldData.Adapt<PositionDetail>());
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
		Position? oldData = await _context.Positions.SingleOrDefaultAsync(b => b.Id == id);
		if (oldData == null)
			return CustomErrors.NotFoundData();

		try
		{
			int effectedRowCount = await _context.Positions.Where(a => a.Id == id).ExecuteDeleteAsync();
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
