using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryManagement.Application.DTOs.Responses;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
       
            private readonly IAuthService _authService;
            public AuthController(IAuthService authService)
            {
                _authService = authService;
            }
            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterDto dto)
            {
                var result = await _authService.RegisterAsync(dto.FullName, dto.Email, dto.Password);
                if (!result.IsSuccess) return BadRequest(new { error = result.Message });
                return Ok(new { message = result.Message });
            }

            [HttpPost("login")]
            public async Task<IActionResult> Login(LoginDto dto)
            {
                var token = await _authService.LoginAsync(dto);
                if (token == null) return Unauthorized(new { error = "E-posta veya şifre hatalı." });
                return Ok(new { token });
            }
        }
    }
