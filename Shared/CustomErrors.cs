using BlogService.Models;

namespace BlogService.Shared;

public class CustomErrors
{
	public static Result InvalidData(object errors = null) => new()
	{
		Message = new()
		{
			Fa = "داده های وارد شده معتبر نمی باشد",
			En = "Data is Invalid!"
		},
		StatusCode = StatusCodes.Status400BadRequest,
		Data = errors,
		Status = false
	};

	public static Result InternalServer(object errors = null) => new()
	{
		Message = new()
		{
			Fa = "با عرض پوزش، عملیات درخواستی با مشکل مواجه شد",
			En = "Operation Failed!"
		},
		StatusCode = StatusCodes.Status500InternalServerError,
		Data = errors,
		Status = false
	};


	public static Result NotAcceptable(object errors = null) => new()
	{
		Message = new()
		{
			Fa = "درخواست شما مجاز نمی باشد!",
			En = "Not Acceptable Request!"
		},
		StatusCode = StatusCodes.Status406NotAcceptable,
		Data = errors,
		Status = false
	};


	public static Result NotFoundData(object errors = null) => new()
	{
		Message = new()
		{
			Fa = "داده ای با مشخصات وارد شده یافت نشد",
			En = "Data not Found!"
		},
		StatusCode = StatusCodes.Status404NotFound,
		Data = errors,
		Status = false
	};

}
