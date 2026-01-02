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
        IResponse<IEnumerable<AuthorQueryDto>> ListAll();
        IResponse<AuthorQueryDto> GetById(int id);
        Task<IResponse<Author>> Create(AuthorCreateDto author);
        Task<IResponse<AuthorUpdateDto>> Update(AuthorUpdateDto author);
        IResponse<Author> Delete(int id);
        IResponse<IEnumerable<AuthorQueryDto>> GetByName(string name);
    }
}
