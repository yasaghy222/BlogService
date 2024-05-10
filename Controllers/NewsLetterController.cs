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
public class NewsLetterController(BlogServiceContext context,
							  IValidator<AddNewsLetterDto> addValidator) : ControllerBase
{
	private readonly NewsLetterService _service = new(context, addValidator);

	[HttpGet]
	[Route("/[controller]")]
	public async Task<IActionResult> Get(int pageIndex = 1, int pageSize = 10)
	{
		Result result = await _service.GetAll(pageIndex, pageSize);
		return StatusCode(result.StatusCode, result.Data);
	}

	[HttpPut]
	public async Task<IActionResult> Put(AddNewsLetterDto model)
	{
		Result result = await _service.Add(model);
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
