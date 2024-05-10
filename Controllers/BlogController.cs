using FluentValidation;
using BlogService.Data;
using BlogService.DTOs;
using BlogService.Models;
using Microsoft.AspNetCore.Mvc;
using BlogService.Enums;

namespace BlogService.Controllers;

[ApiController]
[Route("[controller]")]
public class BlogController(BlogServiceContext context,
							  IValidator<AddBlogDto> addValidator,
							  IValidator<EditBlogDto> editValidator) : ControllerBase
{
	private readonly Services.BlogService _service = new(context, addValidator, editValidator);

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
	public async Task<IActionResult> GetInfo(int pageIndex = 1, int pageSize = 10)
	{
		Result result = await _service.GetAllInfo(pageIndex, pageSize);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Detail")]
	public async Task<IActionResult> GetDetail(int pageIndex = 1, int pageSize = 10, BlogStatus? status = null)
	{
		Result result = await _service.GetAllDetail(pageIndex, pageSize, status);
		return StatusCode(result.StatusCode, result.Data);
	}


	[HttpPut]
	public async Task<IActionResult> Put(AddBlogDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(EditBlogDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/{id}")]
	public async Task<IActionResult> Patch(Guid id, BlogStatus status, string statusDescription)
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
