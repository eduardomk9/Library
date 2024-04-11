using AutoMapper;
using Core.Entities.GenericEnterpise;
using Core.Models;


namespace WebAPI.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {

            CreateMap<Book, BookModel>();
            CreateMap<BookModel, Book>();

            CreateMap<Book, BookModel>()
          .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.BookId));

        }

    }
}
