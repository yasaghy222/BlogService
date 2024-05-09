using BlogService.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlogService.Data;

public static class ModelBuilderConfig
{
	public static void OnModelCreatingBuilder(this ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Blog>(b =>
		{
			b.HasOne(b => b.Category)
			  .WithMany(c => c.Blogs)
			  .HasForeignKey(b => b.CategoryId);

			b.Navigation(c => c.Category).AutoInclude();

			b.HasOne(p => p.Author)
			  .WithMany(a => a.Blogs)
			  .HasForeignKey(p => p.AuthorId);

			b.Navigation(p => p.Author).AutoInclude();
		});

		modelBuilder.Entity<Category>(b =>
	{
		b.HasOne(b => b.Parent)
		  .WithMany(c => c.Chields)
		  .HasForeignKey(b => b.ParentId);
	});


		modelBuilder.Entity<Position>(p =>
		{
			p.HasOne(p => p.Banner)
			  .WithMany(b => b.Positions)
			  .HasForeignKey(p => p.ForeignId)
			  .OnDelete(DeleteBehavior.Cascade);

			p.Navigation(p => p.Banner).AutoInclude();

			p.HasOne(p => p.Blog)
			  .WithMany(b => b.Positions)
			  .HasForeignKey(p => p.ForeignId)
			  .OnDelete(DeleteBehavior.Cascade);

			p.Navigation(p => p.Blog).AutoInclude();

			p.HasOne(p => p.Slide)
			  .WithMany(s => s.Positions)
			  .HasForeignKey(p => p.ForeignId)
			  .OnDelete(DeleteBehavior.Cascade);

			p.Navigation(p => p.Slide).AutoInclude();

			p.HasOne(p => p.Slider)
			  .WithMany(s => s.Positions)
			  .HasForeignKey(p => p.ForeignId)
			  .OnDelete(DeleteBehavior.Cascade);

			p.Navigation(p => p.Slider).AutoInclude();
		});

		modelBuilder.Entity<SliderSlide>(ss =>
			{
				ss.HasOne(ss => ss.Slide)
				  .WithMany(s => s.SliderSlides)
				  .HasForeignKey(b => b.SlideId)
					.OnDelete(DeleteBehavior.Cascade);

				ss.Navigation(ss => ss.Slide).AutoInclude();

				ss.HasOne(ss => ss.Slider)
				  .WithMany(s => s.SliderSlides)
				  .HasForeignKey(b => b.SliderId)
				  .OnDelete(DeleteBehavior.Cascade);

				ss.Navigation(ss => ss.Slider).AutoInclude();
			});
	}
}
