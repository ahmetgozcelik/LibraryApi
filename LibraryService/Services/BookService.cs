using System;
using System.Collections;
using AutoMapper;
using LibraryCore.DTOs;
using LibraryCore.Entities;
using LibraryDataAccess.Repositories;
using LibraryService.Interfaces;
using LibraryService.Response;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LibraryService.Services
{
    public class BookService : IBookService
    {
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IGenericRepository<Book> bookRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IResponse<BookCreateDto>> Create(BookCreateDto book)
        {
            try
            {
                if (book == null)
                {
                    return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Error("Book bilgileri boş olamaz."));
                }

                // 1. Entity'yi oluştur ve kaydet
                var entity = _mapper.Map<Book>(book);
                entity.RecordDate = DateTime.Now;

                _bookRepository.Create(entity);

                _logger.LogInformation(
                    "Kitap oluşturuldu. Id: {Id}, Title: {Title}",
                    entity.Id,
                    entity.Title
                );

                // 2. Entity'yi tekrar DTO'ya Maple (Hata burada çözülüyor)
                var createdDto = _mapper.Map<BookCreateDto>(entity);

                // 3. DTO'yu geri döndür
                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Success(createdDto, "Book başarıyla oluşturuldu."));
            }
            catch
            {
                _logger.LogError("Kitap oluştururken bir hata oluştu.", book.Title);
                return Task.FromResult<IResponse<BookCreateDto>>(ResponseGeneric<BookCreateDto>.Error("Bir hata oluştu."));
            }

        }

        public IResponse<BookQueryDto> Delete(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;

                if (book == null)
                {
                    return ResponseGeneric<BookQueryDto>.Success(null, "Book bulunamadı.");
                }

                _bookRepository.Delete(book);
                _logger.LogInformation($"deleted {book.Title}"); // doğru yazım

                return ResponseGeneric<BookQueryDto>.Success(null, "Book başarıyla silindi.");
            }
            catch
            {
                _logger.LogError($"deleted failed {id}");
                return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<BookQueryDto> GetById(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;

                var bookQueryDto = _mapper.Map<BookQueryDto>(book);

                if (book == null)
                {
                    return ResponseGeneric<BookQueryDto>.Success(null, "Book bulunamadı.");
                }

                return ResponseGeneric<BookQueryDto>.Success(bookQueryDto, "Book başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<BookQueryDto>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<IEnumerable<BookQueryDto>> GetByName(string name)
        {
            try
            {
                var books = _bookRepository.GetAll().Where(x => x.Title == name).ToList();

                var newQueryBooks = _mapper.Map<IEnumerable<BookQueryDto>>(books);

                if (newQueryBooks.Count() == 0 || newQueryBooks == null)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Book bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(newQueryBooks, "Book başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<IEnumerable<BookQueryDto>> ListAll()
        {
            try
            {
                var allBooks = _bookRepository.GetAll().ToList();

                var newQueryBooks = _mapper.Map<IEnumerable<BookQueryDto>>(allBooks);

                if (newQueryBooks == null || newQueryBooks.Count() == 0)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Book bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(newQueryBooks, "Book başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }

        }

        public IResponse<IEnumerable<BookQueryDto>> GetBooksByCategoryId(int categoryId)
        {
            try
            {
                var booksInCategory = _bookRepository.GetAll().Where(x => x.CategoryId == categoryId).ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(booksInCategory);

                if (bookDtos == null || bookDtos.Count() == 0)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitaplar başarıyla döndürüldü.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }
        public IResponse<IEnumerable<BookQueryDto>> GetBooksByAuthorId(int authorId)
        {
            try
            {
                var books = _bookRepository.GetAll().Where(x => x.AuthorId == authorId).ToList();
                var bookDtos = _mapper.Map<IEnumerable<BookQueryDto>>(books);

                if (bookDtos.Count() == 0 || bookDtos == null)
                {
                    return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Kitap bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<BookQueryDto>>.Success(bookDtos, "Kitaplar başarıyla döndürüldü.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookQueryDto>>.Error("Bir hata oluştu.");
            }
        }

        public Task<IResponse<BookUpdateDto>> Update(BookUpdateDto bookUpdateDto)
        {
            try
            {
                //kitabı dbden bul
                var bookEntity = _bookRepository.GetByIdAsync(bookUpdateDto.Id).Result;

                //kitap yoksa hata döndür
                if (bookEntity == null)
                {
                    return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Error("Kitap bulunamadı."));
                }
                _mapper.Map(bookUpdateDto, bookEntity);

                ////kitap varsa güncelle
                //if (!string.IsNullOrEmpty(bookUpdateDto.Title))
                //{
                //    bookEntity.Title = bookUpdateDto.Title;
                //}
                //if (!string.IsNullOrEmpty(bookUpdateDto.Description))
                //{
                //    bookEntity.Description = bookUpdateDto.Description;
                //}
                //if (bookUpdateDto.CountOfPage != null)
                //{
                //    bookEntity.CountOfPage = bookUpdateDto.CountOfPage.Value;
                //}
                //if (bookUpdateDto.AuthorId != null)
                //{
                //    bookEntity.AuthorId = bookUpdateDto.AuthorId.Value;
                //}
                //if (bookUpdateDto.CategoryId != null)
                //{
                //    bookEntity.CategoryId = bookUpdateDto.CategoryId.Value;
                //}

                _bookRepository.Update(bookEntity);

                return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Success(null, "Kitap başarıyla güncellendi."));

            }
            catch
            {
                return Task.FromResult<IResponse<BookUpdateDto>>(ResponseGeneric<BookUpdateDto>.Error("Bir hata oluştu."));
            }
        }
    }
}
