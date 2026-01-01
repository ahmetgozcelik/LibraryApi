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
    public class MapProfile: Profile
    {
        public MapProfile()
        {
            CreateMap<Author, AuthorCreateDto>().ReverseMap();
            CreateMap<Author, AuthorQueryDto>().ReverseMap();

            CreateMap<Book, BookCreateDto>().ReverseMap();
            CreateMap<Book, BookQueryDto>().ReverseMap();

            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Category, CategoryQueryDto>().ReverseMap();
        }
    }
}
