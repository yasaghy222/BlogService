using System.Reflection;
using BlogService.Data;
using BlogService.Mappings;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.RegisterMapsterConfiguration();
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
string? connectionString = builder.Configuration.GetConnectionString("DockerConnection");
builder.Services.AddDbContext<BlogServiceContext>(options => options.UseSqlServer(connectionString));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
