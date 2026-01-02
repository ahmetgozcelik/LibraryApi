using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LibraryCore.DTOs;
using LibraryCore.Entities;
using LibraryDataAccess.Repositories;
using LibraryService.Interfaces;
using LibraryService.Response;

namespace LibraryService.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public Task<IResponse<CategoryCreateDto>> Create(CategoryCreateDto category)
        {
            try
            {
                if (category == null)
                {
                    return Task.FromResult<IResponse<CategoryCreateDto>>(ResponseGeneric<CategoryCreateDto>.Error("Kategori bilgileri boş olamaz."));
                }

                var newCategory = _mapper.Map<Category>(category);
                newCategory.RecordDate = DateTime.Now;

                _categoryRepository.Create(newCategory);

                var createdDto = _mapper.Map<CategoryCreateDto>(newCategory);

                return Task.FromResult<IResponse<CategoryCreateDto>>(ResponseGeneric<CategoryCreateDto>.Success(createdDto, "Kategori başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<CategoryCreateDto>>(ResponseGeneric<CategoryCreateDto>.Error("Bir hata oluştu."));
            }
        }
        

        public IResponse<CategoryQueryDto> Delete(int id)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(id).Result;
                if (category == null)
                {
                    return ResponseGeneric<CategoryQueryDto>.Error("Category bilgileri boş olamaz.");
                }

                _categoryRepository.Delete(category);
                return ResponseGeneric<CategoryQueryDto>.Success(null, "Category başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
            
        }

        public IResponse<CategoryQueryDto> GetById(int id)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(id).Result;
                var newQueryCategory = _mapper.Map<CategoryQueryDto>(category);
                if (newQueryCategory == null)
                {
                    return ResponseGeneric<CategoryQueryDto>.Success(null, "Category bulunamadı.");
                }
                return ResponseGeneric<CategoryQueryDto>.Success(newQueryCategory, "Category başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<CategoryQueryDto>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryQueryDto>> GetByName(string name)
        {
            try
            {
                var categories = _categoryRepository.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower())).ToList();

                var newQueryCategories = _mapper.Map<IEnumerable<CategoryQueryDto>>(categories);

                if (newQueryCategories == null || newQueryCategories.Count() == 0)
                {
                    return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(null, "Category bulunamadı");
                }
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(newQueryCategories, "Category başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
            
        }

        public IResponse<IEnumerable<CategoryQueryDto>> ListAll()
        {
            try
            {
                var allCategories = _categoryRepository.GetAll().ToList();

                var newQueryCategories = _mapper.Map<IEnumerable<CategoryQueryDto>>(allCategories);

                if (newQueryCategories.Count() == 0 || newQueryCategories == null)
                {
                    return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Yazarlar bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Success(newQueryCategories, "Yazarlar listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<CategoryQueryDto>>.Error("Bir hata oluştu.");
            }
           
        }

        public Task<IResponse<CategoryUpdateDto>> Update(CategoryUpdateDto categoryDto)
        {
            try
            {
                var categoryEntity = _categoryRepository.GetByIdAsync(categoryDto.Id).Result;

                if (categoryEntity == null)
                {
                    return Task.FromResult<IResponse<CategoryUpdateDto>>(ResponseGeneric<CategoryUpdateDto>.Error("Kategori bulunamadı."));
                }

                _mapper.Map(categoryDto, categoryEntity);
                _categoryRepository.Update(categoryEntity);

                return Task.FromResult<IResponse<CategoryUpdateDto>>(ResponseGeneric<CategoryUpdateDto>.Success(null, "Kategori başarıyla güncellendi."));
            }
            catch
            {
                return Task.FromResult<IResponse<CategoryUpdateDto>>(ResponseGeneric<CategoryUpdateDto>.Error("Bir hata oluştu."));
            }
        }
    }
}
