using DynamoDBApi.Models;
using DynamoDBApi.Reporitories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDBApi.Services
{
    public class BookService : IBookService
    {
        private readonly IDynamoDbRepository<Book> _dbRepository;

        public BookService(IDynamoDbRepository<Book> dbRepository)
        {
            _dbRepository = dbRepository;
        }
        public async Task<Book>AddBook(Book book)
        {
            return await _dbRepository.Create(book);
        }

        public async Task DeleteBook(string id)
        {
            await _dbRepository.Delete(id);
        }

        public async Task<Book> EditBook(Book book)
        {
            return await _dbRepository.Update(book);
        }

        public async Task<Book> GetBook(string id)
        {
            return await _dbRepository.Get(id);
        }

        public async Task<List<Book>> GetBooks()
        {
            return await _dbRepository.GetAll();
        }
    }
}
