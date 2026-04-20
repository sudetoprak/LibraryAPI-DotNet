using System;
using LibraryManagement.Application.DTOs.Requests;
using LibraryManagement.Application.DTOs.Responses;
using LibraryManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace LibraryManagement.Api.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
        {
            private readonly IAuthorService _authorService;
            public AuthorsController(IAuthorService authorService)
            {
                _authorService = authorService;
            }
            [HttpGet]
            public async Task<ActionResult<List<AuthorDto>>> GetAllAuthors()
            {
                var authors = await _authorService.GetAllAuthorsAsync();
                return Ok(authors);
            }
            [HttpPost]
            public async Task<ActionResult<AuthorDto>> AddAuthor(AuthorCreateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var createdAuthor = await _authorService.AddAuthorAsync(dto);
                return CreatedAtAction(nameof(GetAllAuthors), new { id = createdAuthor.Id }, createdAuthor);
            }
            [HttpPut("{id}")]
            public async Task<ActionResult> UpdateAuthor(int id, AuthorCreateDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _authorService.UpdateAuthorAsync(id, dto);
                if (!result)
                    return NotFound();
                return NoContent();
            }

        }
}
