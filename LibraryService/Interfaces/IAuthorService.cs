using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.DTOs;
using LibraryCore.Entities;
using LibraryDataAccess.Repositories;
using LibraryService.Response;

namespace LibraryService.Interfaces
{
    public interface IAuthorService
    {
        Task<IResponse<IEnumerable<AuthorQueryDto>>> ListAll();
        Task<IResponse<AuthorQueryDto>> GetById(int id);
        Task<IResponse<Author>> Create(AuthorCreateDto author);
        Task<IResponse<AuthorUpdateDto>> Update(AuthorUpdateDto author);
        Task<IResponse<Author>> Delete(int id);
        Task<IResponse<IEnumerable<AuthorQueryDto>>> GetByName(string name);
    }
}
