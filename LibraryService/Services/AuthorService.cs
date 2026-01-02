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

        public Task<IResponse<Author>> Create(AuthorCreateDto author)
        {
            try
            {
                if (author == null)
                {
                    return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Author bilgileri boş olamaz."));
                }

                var newAuthor = _mapper.Map<Author>(author);
                newAuthor.RecordDate = DateTime.Now;

                _authorRepository.Create(newAuthor);

                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(newAuthor, "Yazar başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Bir hata oluştu."));
            }
        }

        public IResponse<Author> Delete(int id)
        {
            try
            {
                //önce entity var mı ona bak
                var author = _authorRepository.GetByIdAsync(id).Result;
                if (author == null)
                {
                    return ResponseGeneric<Author>.Error("Yazar bulunamadı.");
                }

                //entity varsa sil
                _authorRepository.Delete(author);
                return ResponseGeneric<Author>.Success(null, "Yazar başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<Author>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<AuthorQueryDto> GetById(int id)
        {
            try
            {
                var author = _authorRepository.GetByIdAsync(id).Result;

                var authorQueryDto = _mapper.Map<AuthorQueryDto>(author); // mapper ile entity'i query'e dönüştürme

                if (author == null)
                {
                    return ResponseGeneric<AuthorQueryDto>.Success(null, "Yazar bulunamadı.");
                }

                return ResponseGeneric<AuthorQueryDto>.Success(authorQueryDto, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<AuthorQueryDto>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<IEnumerable<AuthorQueryDto>> GetByName(string name)
        {
            try
            {
                var authors = _authorRepository.GetAll().Where(x => x.Name == name).ToList();

                var authorQueryDtos = _mapper.Map < IEnumerable<AuthorQueryDto>>(authors);

                if (authorQueryDtos == null || authorQueryDtos.Count() == 0)
                    return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(null, "Yazar bulunamadı.");
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Success(authorQueryDtos, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<AuthorQueryDto>> ListAll()
        {
            try
            {
                var allAuthors = _authorRepository.GetAll().ToList();

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

        public Task<IResponse<AuthorUpdateDto>> Update(AuthorUpdateDto authorDto)
        {
            try
            {
                var authorEntity = _authorRepository.GetByIdAsync(authorDto.Id).Result;

                if(authorEntity == null)
                {
                    return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Error("Yazar bulunamadı."));
                }

                _mapper.Map(authorDto, authorEntity);
                _authorRepository.Update(authorEntity);

                return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Success(null, "Yazar başarıyla güncellendi."));
            }
            catch
            {
                return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Error("Bir hata oluştu."));
            }
        }
    }
}
