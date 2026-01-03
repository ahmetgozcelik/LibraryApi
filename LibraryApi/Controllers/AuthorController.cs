using LibraryCore.DTOs;
using LibraryCore.Entities;
using LibraryService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [AllowAnonymous]
        [HttpGet("ListAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _authorService.ListAll();

            if(!result.IsSuccess)
            {
                return NotFound(result.Message);
            }
            
            return Ok(result);
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _authorService.Delete(id);

            if(!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] AuthorCreateDto author)
        {
            if(author == null)
            {
                return BadRequest("Yazar bilgileri boş olamaz.");
            }

            var result = await _authorService.Create(author);

            if(!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _authorService.GetByName(name);

            if(!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AuthorUpdateDto author)
        {
            if(author == null)
            {
                return BadRequest("Yazar bilgileri boş olamaz.");
            }

            var result = await _authorService.Update(author);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }
    }
}
