using LibraryCore.DTOs;
using LibraryCore.Entities;
using LibraryService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var authors = _authorService.ListAll();

            if(!authors.IsSuccess)
            {
                return NotFound("Yazar bulunamadı.");
            }
            
            return Ok(authors);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _authorService.Delete(id);

            if(!result.IsSuccess)
            {
                return BadRequest("Silme işlemi başarısız.");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AuthorCreateDto author)
        {
            if(author == null)
            {
                return BadRequest("Yazar bilgileri boş olamaz.");
            }

            var result = _authorService.Create(author);

            if(!result.Result.IsSuccess)
            {
                return BadRequest("Yazar oluşturulamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _authorService.GetByName(name);

            if(!result.IsSuccess)
            {
                return BadRequest("Yazar bulunamadı.");
            }

            return Ok(result);
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] AuthorUpdateDto author)
        {
            if(author == null)
            {
                return BadRequest("Yazar bilgileri boş olamaz.");
            }

            var result = _authorService.Update(author);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Yazar oluşturulamadı.");
            }

            return Ok(result);
        }
    }
}
