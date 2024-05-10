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
			  .HasForeignKey(p => p.ForeignId);

			p.Navigation(p => p.Banner).AutoInclude();

			p.HasOne(p => p.Blog)
			  .WithMany(b => b.Positions)
			  .HasForeignKey(p => p.ForeignId);

			p.Navigation(p => p.Blog).AutoInclude();

			p.HasOne(p => p.Slider)
			  .WithMany(s => s.Positions)
			  .HasForeignKey(p => p.ForeignId);

			p.Navigation(p => p.Slider).AutoInclude();
		});

		modelBuilder.Entity<Slide>(ss =>
			{
				ss.HasOne(s => s.Slider)
					.WithMany(s => s.Slides)
					.HasForeignKey(s => s.SliderId)
					.OnDelete(DeleteBehavior.Cascade);
			});
	}
}
