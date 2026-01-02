using LibraryCore.DTOs;
using LibraryService.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserCreateDto user)
        {
            if(user == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }

            var result = _userService.Create(user);
            if(!result.IsSuccess)
            {
                return BadRequest("Kullanıcı oluşturulamadı.");
            }

            return Ok(result);
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginDto user)
        {
            if (user == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }

            var result = _userService.Login(user);
            if (!result.IsSuccess)
            {
                return BadRequest("Kullanıcı oluşturulamadı.");
            }

            return Ok(result);
        }
    }
}
