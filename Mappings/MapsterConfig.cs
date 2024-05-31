using System.Reflection;
using BlogService.Entities;
using BlogService.Models;
using Mapster;
namespace BlogService.Mappings;

public static class MapsterConfig
{
	public static void RegisterMapsterConfiguration(this IServiceCollection services)
	{
		TypeAdapterConfig<Blog, BlogDetail>.NewConfig()
		.Map(dto => dto.CategoryTitle, s => s.Category.Title)
		.Map(dto => dto.CategoryEnTitle, s => s.Category.EnTitle);

		TypeAdapterConfig<Blog, BlogDetail>.NewConfig()
		.Map(dto => dto.CategoryTitle, s => s.Category.Title)
		.Map(dto => dto.CategoryEnTitle, s => s.Category.EnTitle);

		TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
	}
}
