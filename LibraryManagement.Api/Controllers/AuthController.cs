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
    //Bu controller, kullanıcıların kayıt ve giriş işlemlerini yönetir
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        // Kullanıcı kayıt ve giriş işlemleri IauthService arayüzü kullanılarak servis katmanında gercekleşir. 
        private readonly IAuthService _authService;

        //Authcontroller calısırken IAuthService nesnesi constructor uzerinden alınır
        //Bu sayede kayıt ve giriş işlemleri authservise kullanılabilir.
        public AuthController(IAuthService authService)
            {
                _authService = authService;
            }




            [HttpPost("register")]
            public async Task<IActionResult> Register(RegisterDto dto)
            {
            //Kullanıcıdan gelen (fullname,email ve password )bilgileri authservise katmanına gonderilir ve işlem gercekleşir
          
                var result = await _authService.RegisterAsync(dto.FullName, dto.Email, dto.Password);

            //eger kayıt işlemi basarsız olursa, BadRequest ile hata mesajı döndürülür. Eğer başarılı olursa, Ok ile başarı mesajı döndürülür.
            if (!result.IsSuccess) return BadRequest(new { error = result.Message });
                return Ok(new { message = result.Message });
            }




        
        [HttpPost("login")]
            public async Task<IActionResult> Login(LoginDto dto)
            {
            //Kullanıcının girdiği eposta ve sifre bilgielri
            //authservice katmanına gonderilir

            var token = await _authService.LoginAsync(dto);


            //Eğer giriş işlemi başarılı olursa, authservice tarafından oluşturulan JWT token döndürülür. Eğer giriş işlemi başarısız olursa, Unauthorized ile hata mesajı döndürülür.
            if (token == null) return Unauthorized(new { error = "E-posta veya şifre hatalı." });
                return Ok(new { token });
            }
        }
    }
