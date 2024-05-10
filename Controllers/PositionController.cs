using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BlogService.DTOs;
using BlogService.Models;
using BlogService.Enums;
using BlogService.Data;
using BlogService.Services;

namespace BlogService.Controllers;

[ApiController]
[Route("[controller]")]
public class PositionController(BlogServiceContext context,
							  IValidator<AddPositionDto> addValidator,
							  IValidator<EditPositionDto> editValidator) : ControllerBase
{
	private readonly PositionService _service = new(context, addValidator, editValidator);

	[HttpGet]
	[Route("/[controller]/{id}")]
	public async Task<IActionResult> Get(Guid id, string key)
	{
		Result result = await _service.GetDetail(id, key);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Info")]
	public async Task<IActionResult> GetInfo(string key, PositionType? type)
	{
		Result result = await _service.GetAllInfo(key, type);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Detail")]
	public async Task<IActionResult> GetDetail(int pageIndex = 1, int pageSize = 10, string key = "", PositionType? type = null, PositionStatus? status = null)
	{
		Result result = await _service.GetAllDetail(pageIndex, pageSize, key, type, status);
		return StatusCode(result.StatusCode, result.Data);
	}


	[HttpPut]
	public async Task<IActionResult> Put(AddPositionDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(EditPositionDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/{id}")]
	public async Task<IActionResult> Patch(Guid id, PositionStatus status)
	{
		Result result = await _service.ChangeStatus(id, status);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpDelete]
	[Route("/[controller]/{id}")]
	public async Task<IActionResult> Delete(Guid id)
	{
		Result result = await _service.Delete(id);
		return StatusCode(result.StatusCode, result.Data);
	}
}
