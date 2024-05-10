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
public class CategoryController(BlogServiceContext context,
							  IValidator<AddCategoryDto> addValidator,
							  IValidator<EditCategoryDto> editValidator) : ControllerBase
{
	private readonly CategoryService _service = new(context, addValidator, editValidator);

	[HttpGet]
	[Route("/[controller]/{id}")]
	public async Task<IActionResult> Get(Guid id)
	{
		Result result = await _service.Get(id);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpGet]
	[Route("/[controller]")]
	public async Task<IActionResult> GetInfo()
	{
		Result result = await _service.GetAll();
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put(AddCategoryDto model)
	{
		Result result = await _service.Add(model);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPost]
	public async Task<IActionResult> Post(EditCategoryDto model)
	{
		Result result = await _service.Edit(model);
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
