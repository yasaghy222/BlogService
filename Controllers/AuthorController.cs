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
public class AuthorController(BlogServiceContext context,
							  IValidator<AddAuthorDto> addValidator,
							  IValidator<EditAuthorDto> editValidator) : ControllerBase
{
	private readonly AuthorService _service = new(context, addValidator, editValidator);

	[HttpGet]
	[Route("/[controller]/Info/{id}")]
	public async Task<IActionResult> GetInfo(Guid id)
	{
		Result result = await _service.GetInfo(id);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Detail/{id}")]
	public async Task<IActionResult> GetDetail(Guid id)
	{
		Result result = await _service.GetDetail(id);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Info")]
	public async Task<IActionResult> GetInfo()
	{
		Result result = await _service.GetAllInfo();
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Detail")]
	public async Task<IActionResult> GetDetail(int pageIndex = 1, int pageSize = 10, AuthorStatus? status = null)
	{
		Result result = await _service.GetAllDetail(pageIndex, pageSize, status);
		return StatusCode(result.StatusCode, result.Data);
	}


	[HttpPut]
	public async Task<IActionResult> Put(AddAuthorDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(EditAuthorDto model)
	{
		Result result = await _service.Edit(model);
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
