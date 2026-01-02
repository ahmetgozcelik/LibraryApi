using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryCore.Entities;
using LibraryCore.DTOs;

namespace LibraryService.MapProfile
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            #region Author Mappings
            CreateMap<Author, AuthorCreateDto>().ReverseMap();
            CreateMap<Author, AuthorQueryDto>().ReverseMap();
            CreateMap<AuthorUpdateDto, Author>()
                .IgnoreNullAndEmpty();
            #endregion

            #region Book Mappings
            CreateMap<Book, BookCreateDto>().ReverseMap();
            CreateMap<Book, BookQueryDto>().ReverseMap();
            CreateMap<BookUpdateDto, Book>()
                .IgnoreNullAndEmpty();
            #endregion

            #region Category Mappings
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryQueryDto>().ReverseMap();
            CreateMap<CategoryUpdateDto, Category>()
                .IgnoreNullAndEmpty();
            #endregion

            //#region User Mappings
            //CreateMap<User, UserCreateDto>().ReverseMap();
            //CreateMap<User, UserLoginDto>().ReverseMap();
            //#endregion
        }
    }
}
