using System;
using System.Collections;
using AutoMapper;
using LibraryCore.DTOs;
using LibraryCore.Entities;
using LibraryDataAccess.Repositories;
using LibraryService.Interfaces;
using LibraryService.Response;
using Microsoft.Extensions.Logging;

namespace LibraryService.Services
{
    public class BookService: IBookService
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
                var newBook = _mapper.Map<Book>(book);
                newBook.RecordDate = DateTime.Now;
                _bookRepository.Create(newBook);

                _logger.LogInformation("Kitap başarıyla oluşturuldu.", book.Title);

                // 2. Entity'yi tekrar DTO'ya Maple (Hata burada çözülüyor)
                var createdDto = _mapper.Map<BookCreateDto>(newBook);

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

        public Task<IResponse<BookQueryDto>> Update(BookQueryDto book)
        {
            throw new NotImplementedException();
        }
    }
}
