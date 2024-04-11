
using Core.DTOs.Book;
using Core.Entities.GenericEnterpise;
using Core.Models;

namespace Core.Business
{
    public interface IBookBusiness
    {
        Task<IEnumerable<BookModel>> GetBooksAsync(GetBooksDto getBooksDto);
    }
}
