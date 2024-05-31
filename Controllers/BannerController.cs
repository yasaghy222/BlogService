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
public class BannerController(BlogServiceContext context,
							  IValidator<AddBannerDto> addValidator,
							  IValidator<EditBannerDto> editValidator,
							  IValidator<AddFileDto> fileValidator) : ControllerBase
{
	private readonly BannerService _service = new(context, addValidator, editValidator, fileValidator);

	[HttpGet]
	[Route("/[controller]/{id}/{type}")]
	public async Task<IActionResult> GetInfo(Guid id, GetDataType type)
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
	public async Task<IActionResult> GetDetail(int pageIndex = 1, int pageSize = 10, BannerStatus? status = null)
	{
		Result result = await _service.GetAllDetail(pageIndex, pageSize, status);
		return StatusCode(result.StatusCode, result.Data);
	}


	[HttpPut]
	public async Task<IActionResult> Put(EditBannerDto model)
	{
		Result result = await _service.Edit(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(AddBannerDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPatch]
	[Route("/[controller]/{id}/{status}")]
	public async Task<IActionResult> Patch(Guid id, BannerStatus status)
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
