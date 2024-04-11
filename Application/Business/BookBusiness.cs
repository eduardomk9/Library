using AutoMapper;
using Core.Business;
using Core.DTOs.Book;
using Core.Entities.GenericEnterpise;
using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;



namespace Application.Business
{
    public class BookBusiness(
         IMapper mapper,
         IGenericEnterpriseRepository<Book> repositoryBook
         ) : IBookBusiness
    {
        private readonly IMapper _mapper = mapper;
        private readonly IGenericEnterpriseRepository<Book> _repositoryGEBook = repositoryBook;

        public async Task<IEnumerable<BookModel>> GetBooksAsync(GetBooksDto getBooksDto)
        {
            try
            {
                Expression<Func<Book, bool>> filter = null!;

                switch (getBooksDto.Type.ToLower())
                {
                    case "title":
                        filter = book => EF.Functions.Like(book.Title.ToLower(), $"%{getBooksDto.Value.ToLower()}%");
                        break;
                    case "firstname":
                        filter = book => EF.Functions.Like(book.FirstName.ToLower(), $"%{getBooksDto.Value.ToLower()}%");
                        break;
                    case "lastname":
                        filter = book => EF.Functions.Like(book.LastName.ToLower(), $"%{getBooksDto.Value.ToLower()}%");
                        break;
                    case "isbn":
                        filter = book => book.Isbn == null || book.Isbn.Contains(getBooksDto.Value); // Handle null Isbn
                        break;
                    case "category":
                        filter = book => book.Category == null || book.Category.Contains(getBooksDto.Value); // Handle null Category
                        break;
                    case "type":
                        filter = book => book.Type == null || book.Type.Contains(getBooksDto.Value); // Handle null Category
                        break;
                    default:
                        throw new ArgumentException($"Invalid search type: {getBooksDto.Type}");
                }
                IEnumerable<Book> books = await _repositoryGEBook.GetAll(filter);
                IEnumerable<BookModel> bookModels = _mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(books);

                return bookModels;

            }
            catch (Exception ex)
            {
                throw new Exception($"BookBusiness | GetBooksAsync | {ex.Message}");
            }
        }
    }
}
