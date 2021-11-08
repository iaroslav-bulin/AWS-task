using DynamoDBApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DynamoDBApi.Reporitories
{
    public interface IDynamoDbRepository<DataModel> where DataModel : Book
    {
        public Task<List<DataModel>> GetAll();
        public Task<DataModel> Get(string id);
        public Task<DataModel> Create(DataModel model);
        public Task<DataModel> Update(DataModel model);
        public Task Delete(string id);
    }
}
