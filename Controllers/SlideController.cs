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
public class SlideController(BlogServiceContext context,
							  IValidator<AddSlideDto> addValidator,
							  IValidator<EditSlideDto> editValidator,
							  IValidator<AddFileDto> fileValidator) : ControllerBase
{
	private readonly SlideService _service = new(context, addValidator, editValidator, fileValidator);

	[HttpGet]
	[Route("/[controller]/{sliderId}")]
	public async Task<IActionResult> Get(Guid sliderId, SlideStatus? status)
	{
		Result result = await _service.GetAllDetail(sliderId, status);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put(EditSlideDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(AddSlideDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/{id}/{status}")]
	public async Task<IActionResult> Patch(Guid id, SlideStatus status)
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
