using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.DTOs;
using LibraryCore.Entities;

namespace LibraryService.Interfaces
{
    public interface IUserService
    {
        IResponse<UserCreateDto> Create(UserCreateDto userCreateDto);
        IResponse<string> Login(UserLoginDto userLoginDto);
    }
}
