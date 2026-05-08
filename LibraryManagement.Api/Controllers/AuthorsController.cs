using System;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LibraryManagement.Api.Controllers

{

    //Bu sınıf Yazarlari listeleme ve ekleme ve mevcut yazarı guncelleme işlemlerini yönetir.

    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
        {

        //Yazar işlemleri IAuthorService arayüzü kullanılarak servis katmanında gercekleşir.
        private readonly IAuthorService _authorService;
            public AuthorsController(IAuthorService authorService)
            {
                _authorService = authorService;
            }



            //Tum yazarları listelemek ıcın kullanılır
        [HttpGet]
            public async Task<ActionResult<PagedResult<AuthorDto>>> GetAllAuthors(int page = 1, int pageSize = 10)
            {
            //page ve page size sayesinde sayfalama yapılır 
            //servvis katmanına gidilerek  veritabanındakı yazarlar page ve page size ile alınır.
            var authors = await _authorService.GetAllAuthorsAsync(page, pageSize);

            //listeleme basarılı olursa , Ok ile yazarların listesi döndürülür.
            return Ok(authors);
            }




       
            [HttpPost]
        // yeni bir yazar eklemek icin kullanilir.
        public async Task<ActionResult<AuthorDto>> AddAuthor(AuthorCreateDto dto)
            {

            // kullanicidan gelen verilerin dogrulugu kontrol edilir.
            // eger model kuru-allarına uygun deilse BadRequest ile hata mesajı döndürülür.
            if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var createdAuthor = await _authorService.AddAuthorAsync(dto);
                return CreatedAtAction(nameof(GetAllAuthors), new { id = createdAuthor.Id }, createdAuthor);
            }



            [HttpPut("{id}")]
        // mevcut bir yazarı güncellemek için kullanılır.
        public async Task<ActionResult> UpdateAuthor(int id, AuthorCreateDto dto)
            {

            //kullanıcıdan gelen verilerin doğruluğu kontrol edilir.
            //eğer model kurallarına uygun değilse BadRequest ile hata mesajı döndürülür.
            if (!ModelState.IsValid)
                    return BadRequest(ModelState);

            //servis katmanına gidilerek, id ve güncellenmiş yazar bilgileri ile güncelleme işlemi gerçekleştirilir. 
            var result = await _authorService.UpdateAuthorAsync(id, dto);
                if (!result)
                    return NotFound();
                return NoContent();
            }

        }
}
