using BlogService.Models;

namespace BlogService.Shared;
public class CustomResults
{
	public static Result SuccessOperation(object? data = null) => new()
	{
		Message = new()
		{
			Fa = "عملیات با موفقیت انجام شد",
			En = "Operation is Success"
		},
		StatusCode = StatusCodes.Status200OK,
		Data = data
	};

	public static Result SuccessDelete() => new()
	{
		Message = new()
		{
			Fa = "عملیات حذف با موفقیت انجام شد",
			En = "Delete Operation is Success"
		},
		StatusCode = StatusCodes.Status204NoContent,
		Data = null
	};


	public static Result SuccessCreation(object data) => new()
	{
		Message = new()
		{
			Fa = "عملیات ایجاد داده با موفقیت انجام شد",
			En = "Create Operation is Success"
		},
		StatusCode = StatusCodes.Status201Created,
		Data = data
	};

	public static Result SuccessUpdate(object data) => new()
	{
		Message = new()
		{
			Fa = "عملیات ویرایش داده با موفقیت انجام شد",
			En = "Update Operation is Success"
		},
		StatusCode = StatusCodes.Status202Accepted,
		Data = data
	};
}