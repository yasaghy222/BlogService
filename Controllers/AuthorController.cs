using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BlogService.DTOs;
using BlogService.Models;
using BlogService.Enums;
using BlogService.Data;
using BlogService.Services;
using FileService;

namespace BlogService.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthorController(BlogServiceContext context,
							  IValidator<AddAuthorDto> addValidator,
							  IValidator<EditAuthorDto> editValidator,
							  IValidator<AddFileDto> fileValidator) : ControllerBase
{
	private readonly AuthorService _service = new(context, addValidator, editValidator, fileValidator);

	[HttpGet]
	[Route("/[controller]/{id}/{type}")]
	public async Task<IActionResult> GetInfo(Guid id, GetDataType type)
	{
		Result result = type switch
		{
			GetDataType.Info => await _service.GetInfo(id),
			GetDataType.Detail => await _service.GetDetail(id),
			_ => await _service.GetInfo(id),
		};

		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/{type}")]
	public async Task<IActionResult> Get(GetDataType type, int pageIndex, int pageSize, AuthorStatus? status)
	{

		Result result = type switch
		{
			GetDataType.Info => await _service.GetAllInfo(pageIndex, pageSize),
			GetDataType.Detail => await _service.GetAllDetail(pageIndex, pageSize, status),
			_ => await _service.GetAllInfo(pageIndex, pageSize),
		};

		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put(EditAuthorDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(AddAuthorDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/{id}")]
	public async Task<IActionResult> Patch(Guid id, AuthorStatus status, string statusDescription)
	{
		Result result = await _service.ChangeStatus(id, status, statusDescription);
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
