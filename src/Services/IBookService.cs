using DynamoDBApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDBApi.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooks();
        Task<Book> GetBook(string id);
        Task<Book> AddBook(Book book);
        Task<Book> EditBook(Book book);
        Task DeleteBook(string id);
    }
}
