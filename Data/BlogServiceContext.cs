using BlogService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Data;

public class BlogServiceContext(DbContextOptions<BlogServiceContext> options) : DbContext(options)
{
	public DbSet<Author> Authors { get; set; }
	public DbSet<Banner> Banners { get; set; }
	public DbSet<Blog> Blogs { get; set; }
	public DbSet<Category> Categories { get; set; }
	public DbSet<NewsLetter> NewsLetters { get; set; }
	public DbSet<Position> Positions { get; set; }
	public DbSet<Slide> Slides { get; set; }
	public DbSet<Slider> Sliders { get; set; }
	public DbSet<SliderSlide> SliderSlides { get; set; }


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.OnModelCreatingBuilder();
	}
}
