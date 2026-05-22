using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using System.Security.Cryptography;
using System.IO;


using LibraryManagement.Application.DTOs.Responses;

namespace LibraryManagement.Application.Services
{
    // AuthService sınıfı kullanıcı kayıt ve giriş işlemlerinin yapıldığı servis sınıfıdır.
    // Kullanıcı kayıt olurken e-posta kontrolü yapılır, şifre güvenli şekilde hashlenir.
    // Kullanıcı giriş yaptığında ise e-posta ve şifre doğrulanır, bilgiler doğruysa JWT token oluşturulur.
    public class AuthService : IAuthService
    {

        // appsettings.json içindeki JWT ayarlarına ulaşmak için IConfiguration kullanılır.
        // Token oluştururken key, issuer ve audience bilgileri buradan alınır.
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;


        public AuthService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        // Kullanıcının sisteme kayıt olması için kullanılan metottur.
        public async Task<ServiceResult> RegisterAsync(string fullName, string email, string password)
        {
            var exists = await _context.Users.AnyAsync(u => u.Email == email);
            if (exists) return ServiceResult.Failure("Bu e-posta adresi zaten kayıtlı.");

            var user = new User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                RoleId = 3
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return ServiceResult.Success("Kayıt başarılı!");
        }
        // Kullanıcının sisteme giriş yapması için kullanılan metottur.
        public async Task<string?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null) return null;
 
            // Şifre doğrulama
            var isValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!isValid) return null;

            return GenerateToken(user);
        }




        // Kullanıcı bilgilerine göre JWT token oluşturmak için kullanılan metottur.
        private string GenerateToken(User user)
        {
            // Token oluşturulurken kullanılan gizli anahtar, issuer ve audience bilgileri appsettings.json dosyasından alınır.
            //tokenen sahte olup olmadığını anlamaya yarar

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));



            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, user.Role?.Name ?? "Member")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        

    }
}