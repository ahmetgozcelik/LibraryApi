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
using Microsoft.EntityFrameworkCore;

namespace LibraryService.Services
{
    public class AuthorService : IAuthorService
    {
        // DI ile GenericRepositoryi alımı
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IGenericRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<IResponse<Author>> Create(AuthorCreateDto authorCreateDto)
        {
            try
            {
                if (authorCreateDto == null)
                {
                    return ResponseGeneric<Author>.Error("Author bilgileri boş olamaz.");
                }

                var authorEntity = _mapper.Map<Author>(authorCreateDto);
                authorEntity.RecordDate = DateTime.Now;

                await _authorRepository.CreateAsync(authorEntity);

                return ResponseGeneric<Author>.Success(authorEntity, "Yazar başarıyla oluşturuldu.");
            }
            catch
            {
                return ResponseGeneric<Author>.Error("Bir hata oluştu.");
            }
        }

        public async Task<IResponse<Author>> Delete(int id)
        {
            try
            {
                //önce entity var mı ona bak
                var author =await _authorRepository.GetByIdAsync(id);
                if (author == null)
                {
                    return ResponseGeneric<Author>.Error("Yazar bulunamadı.");
                }

                //entity varsa sil
                await _authorRepository.DeleteAsync(author);
                return ResponseGeneric<Author>.Success(null, "Yazar başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<Author>.Error("Bir hata oluştu.");
            }

        }

        public async Task<IResponse<AuthorQueryDto>> GetById(int id)
        {
            try
            {
                var author = await _authorRepository.GetByIdAsync(id);

                if (author == null)
                {
                    return ResponseGeneric<AuthorQueryDto>.Success(null, "Yazar bulunamadı.");
                }

                var authorQueryDto = _mapper.Map<AuthorQueryDto>(author); // mapper ile entity'i query'e dönüştürme


                return ResponseGeneric<AuthorQueryDto>.Success(authorQueryDto, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<AuthorQueryDto>.Error("Bir hata oluştu.");
            }

        }

        public async Task<IResponse<IEnumerable<AuthorQueryDto>>> GetByName(string name)
        {
            try
            {
                var authors = await _authorRepository.GetAll().Where(x => x.Name == name).ToListAsync();

                var authorQueryDtos = _mapper.Map <IEnumerable<AuthorQueryDto>>(authors);

                if (authorQueryDtos == null || authorQueryDtos.Count() == 0)
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(null, "Yazar bulunamadı.");

                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public async Task<IResponse<IEnumerable<AuthorQueryDto>>> ListAll()
        {
            try
            {
                var allAuthors = await _authorRepository.GetAll().ToListAsync();

                var authorQueryDtos = _mapper.Map<IEnumerable<AuthorQueryDto>>(allAuthors);

                if (allAuthors.Count == 0 || allAuthors == null)
                {
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Yazarlar bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazarlar listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
            
        }

        public async Task<IResponse<AuthorUpdateDto>> Update(AuthorUpdateDto authorDto)
        {
            try
            {
                var authorEntity = await _authorRepository.GetByIdAsync(authorDto.Id);

                if(authorEntity == null)
                {
                    return ResponseGeneric<AuthorUpdateDto>.Error("Yazar bulunamadı.");
                }

                _mapper.Map(authorDto, authorEntity);
                await _authorRepository.UpdateAsync(authorEntity);

                return ResponseGeneric<AuthorUpdateDto>.Success(null, "Yazar başarıyla güncellendi.");
            }
            catch
            {
                return ResponseGeneric<AuthorUpdateDto>.Error("Bir hata oluştu.");
            }
        }
    }
}
