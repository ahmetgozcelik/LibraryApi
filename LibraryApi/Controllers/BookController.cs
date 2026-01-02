using LibraryCore.DTOs;
using LibraryCore.Entities;
using LibraryService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("/api[controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("ListAll")]
        public IActionResult ListAll()
        {
            var books = _bookService.ListAll();

            if(!books.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(books);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var book = _bookService.Delete(id);

            if (!book.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(book);
        }

        // async await denedim.
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] BookCreateDto book)
        {
            if(book == null)
            {
                return BadRequest("Kitap bilgileri boş olamaz.");
            }

            var result = await _bookService.Create(book);

            if (!result.IsSuccess)
            {
                return BadRequest("Yazar oluşturulamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var book = _bookService.GetByName(name);

            if(!book.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(book);
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] BookUpdateDto book)
        {
            if(book == null)
            {
                return BadRequest("Kitap bilgileri boş olamaz.");
            }

            var result = _bookService.Update(book);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Kitap güncellenemedi.");
            }

            return Ok(book);
        }

        [HttpGet("GetBooksByCategoryId")]
        public IActionResult GetBooksByCategoryId(int categoryId)
        {
            var result = _bookService.GetBooksByCategoryId(categoryId);

            if (!result.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetBooksByAuthorId")]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var result = _bookService.GetBooksByAuthorId(authorId);

            if (!result.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(result);
        }
    }
}
