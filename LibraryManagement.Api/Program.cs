using LibraryManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Api.Middlewares;
using LibraryManagement.Application;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IBookService, BookService>(); 
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();