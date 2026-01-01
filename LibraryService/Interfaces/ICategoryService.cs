using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.DTOs;
using LibraryCore.Entities;

namespace LibraryService.Interfaces
{
    public interface ICategoryService
    {
        IResponse<IEnumerable<CategoryQueryDto>> ListAll();
        IResponse<CategoryQueryDto> GetById(int id);
        Task<IResponse<CategoryCreateDto>> Create(CategoryCreateDto category);
        Task<IResponse<CategoryQueryDto>> Update(CategoryQueryDto category);
        IResponse<CategoryQueryDto> Delete(int id);
        IResponse<IEnumerable<CategoryQueryDto>> GetByName(string name);
    }
}
