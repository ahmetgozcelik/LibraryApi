using LibraryCore.DTOs;
using LibraryService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("ListAll")]
        public IActionResult ListAll()
        {
            var categories = _categoryService.ListAll();

            if (!categories.IsSuccess)
            {
                return NotFound("Kategori bulunamadı.");
            }

            return Ok(categories);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {
            var result = _categoryService.Delete(id);

            if (!result.IsSuccess)
            {
                return BadRequest("Kategori silinemedi.");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryCreateDto category)
        {
            if (category == null)
            {
                return BadRequest("Kategori bilgileri boş olamaz.");
            }

            var result = _categoryService.Create(category);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Kategori oluşturalamadı.");
            }

            return Ok(result);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _categoryService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound("Kategori bulunamadı.");
            }

            return Ok(result);
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] CategoryUpdateDto category)
        {
            if(category == null)
            {
                return BadRequest("Kategori bilgileri boş olamaz.");
            }

            var result = _categoryService.Update(category);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Kategori işlemi başarısız.");
            }

            return Ok(result);
        }
    }
}
