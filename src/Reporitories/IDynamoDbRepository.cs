using DynamoDBApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDBApi.Reporitories
{
    public interface IDynamoDbRepository
    {
        public Task<List<Book>> GetAll();
        public Task<Book> Get(string id);
        public Task<Book> Create(Book model);
        public Task<Book> Update(Book model);
        public Task Delete(string id);
    }
}
