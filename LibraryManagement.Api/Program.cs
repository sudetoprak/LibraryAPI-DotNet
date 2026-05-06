using LibraryManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Api.Middlewares;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using LibraryManagement.Application.Validators;
using LibraryManagement.Api.Middleware;


// builder nesnesi olusur  Bu asamada, veritabanı bağlantısı, servisler, doğrulama kurallari, kimlik doğrulama ve Swagger gibi özellikler  bu nesne uzerinden eklenir.
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// veritabanı baglantisi eklenir. Bu asamada, SQL Server veritabanına bağlantı sağlanır. MigrationsAssembly , Infrastructure katmanında bulunur.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString,
        b => b.MigrationsAssembly("LibraryManagement.Infrastructure")));


//cors ayarlarının erişimini apıden gelen isteğe sınırsız ierişim verir.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});

// Uygulama servisleri eklenir. Bu asamada, uygulamanın iş mantığını gerçekleştiren servisler eklenir. Her bir servis, ilgili arayüzü ile birlikte kaydedilir.
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>();


//  FluentValidation kütüphanesi kullanılarak doğrulama kuralları eklenir.
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<BookCreateDtoValidator>();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value?.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors.Select(error => error.ErrorMessage))
            .ToList();

        return new BadRequestObjectResult(new
        {
            status = StatusCodes.Status400BadRequest,
            message = "Gönderilen bilgiler geçersiz.",
            errors
        });
    };
});

// JWT kullanarak kullanıcıların kimlik doğrulaması sağlanır. TokenValidationParameters, token'ın geçerliliğini kontrol etmek için kullanılan parametreleri içerir. Bu parametreler arasında issuer (token'ı oluşturan), audience (token'ın hedef kitlesi), lifetime (token'ın geçerlilik süresi) ve signing key (token'ı imzalamak için kullanılan anahtar) gibi bilgiler bulunur.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });


// Swagger ve diğer API özellikleri eklenir. Bu asamada, API'nin kontrolcülerini ekleyerek, Swagger ile API dokümantasyonu oluşturulur 
builder.Services.AddControllers();

// SwaggerGen, API'nin kontrolcülerini tarayarak, API'nin nasıl kullanılacağını gösteren bir kullanıcı arayüzü oluşturur.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// app nesnesi uygulamanın çalıştırılmasını sağlar.Swagger'ın etkinleştirilmesi, statik dosyaların sunulması, HTTPS yönlendirmesi, CORS politikalarının uygulanması, özel middleware'lerin eklenmesi, kimlik doğrulama ve yetkilendirme işlemlerinin gerçekleştirilmesi ve API kontrolcülerinin haritalanması gibi işlemler yapılır.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseCors("AllowAll");
app.UseMiddleware<ExceptionMiddleware>(); 
app.UseMiddleware<RequestResponseLoggingMiddleware>();
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
