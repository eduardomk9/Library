
using Core.Entities.GenericEnterpise;
using Core.Models;

namespace Core.Business
{
    public interface IBookBusiness
    {
        Task<IList<BookModel>> GetBooksAsync();
    }
}
