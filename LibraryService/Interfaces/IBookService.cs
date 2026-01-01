using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.DTOs;
using LibraryCore.Entities;

namespace LibraryService.Interfaces
{
    public interface IBookService
    {
        IResponse<IEnumerable<BookQueryDto>> ListAll();
        IResponse<BookQueryDto> GetById(int id);
        Task<IResponse<BookCreateDto>> Create(BookCreateDto book);
        Task<IResponse<BookQueryDto>> Update(BookQueryDto book);
        IResponse<BookQueryDto> Delete(int id);
        IResponse<IEnumerable<BookQueryDto>> GetByName(string name);
    }
}
