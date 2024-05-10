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
public class SliderController(BlogServiceContext context,
							  IValidator<AddSliderDto> addValidator,
							  IValidator<EditSliderDto> editValidator) : ControllerBase
{
	private readonly SliderService _service = new(context, addValidator, editValidator);

	[HttpGet]
	[Route("/[controller]/Info/{id}")]
	public async Task<IActionResult> GetInfo(Guid id, string key)
	{
		Result result = await _service.GetInfo(id, key);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Detail/{id}")]
	public async Task<IActionResult> GetDetail(Guid id, string key)
	{
		Result result = await _service.GetDetail(id, key);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Info")]
	public async Task<IActionResult> GetInfo(string key)
	{
		Result result = await _service.GetAllInfo(key);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]/Detail")]
	public async Task<IActionResult> GetDetail(string key = "", SliderStatus? status = null)
	{
		Result result = await _service.GetAllDetail(key, status);
		return StatusCode(result.StatusCode, result.Data);
	}


	[HttpPut]
	public async Task<IActionResult> Put(AddSliderDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(EditSliderDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/{id}/{status}")]
	public async Task<IActionResult> Patch(Guid id, SliderStatus status)
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
