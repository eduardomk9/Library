using AutoMapper;
using Core.Business;
using Core.Entities.GenericEnterpise;
using Core.Models;
using Core.Repositories;

namespace Application.Business
{
    public class BookBusiness(
         IMapper mapper,
         IGenericEnterpriseRepository<Book> repositoryBook
         ) : IBookBusiness
    {
        private readonly IMapper _mapper = mapper;
        private readonly IGenericEnterpriseRepository<Book> _repositoryGEBook = repositoryBook;

        public async Task<IList<BookModel>> GetBooksAsync()
        {
            try
            {
                IEnumerable<Book> books = await _repositoryGEBook.GetAll();
                List<BookModel> BookList = _mapper.Map<List<BookModel>>(books);
                return BookList;
            }
            catch (Exception ex)
            {
                throw new Exception($"BookBusiness | GetBooksAsync | {ex.Message}");
            }
        }
    }
}
