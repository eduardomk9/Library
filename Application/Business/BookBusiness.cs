using AutoMapper;
using Core.Business;
using Core.DTOs.Book;
using Core.Entities.GenericEnterpise;
using Core.Repositories;
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

        public async Task<IEnumerable<Book>> GetBooksAsync(GetBooksDto getBooksDto)
        {
            try
            {
                Expression<Func<Book, bool>> filter = null!;

                switch (getBooksDto.Type.ToLower())
                {
                    case "title":
                        filter = book => book.Title.Contains(getBooksDto.Value, StringComparison.CurrentCultureIgnoreCase);
                        break;
                    case "firstname":
                        filter = book => book.FirstName.Contains(getBooksDto.Value, StringComparison.CurrentCultureIgnoreCase);
                        break;
                    case "lastname":
                        filter = book => book.LastName.Contains(getBooksDto.Value, StringComparison.CurrentCultureIgnoreCase);
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
                IEnumerable<Book> books =  await _repositoryGEBook.GetAll(filter);

                return books;
            }
            catch (Exception ex)
            {
                throw new Exception($"BookBusiness | GetBooksAsync | {ex.Message}");
            }
        }
    }
}
