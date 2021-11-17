using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using DynamoDBApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DynamoDBApi.Reporitories
{
    public class DynamoDbRepository : IDynamoDbRepository
    {
        private readonly string _isbn = "ISBN";
        private readonly string _title = "Title";
        private readonly string _desc = "Description";
        private string _tableName = "Books";

        private IAmazonDynamoDB AmazonDynamoDBClient { get; set; }
        public DynamoDbRepository(IAmazonDynamoDB amazonDynamoDB)
        {
            AmazonDynamoDBClient = amazonDynamoDB;
        }

        public async Task<Book> Create(Book model)
        {
            var request = new PutItemRequest
            {
                TableName = _tableName,
                Item = new Dictionary<string, AttributeValue>()
                {
                    { _title, new AttributeValue { S = model.Title }},
                    { _isbn, new AttributeValue { S = model.ISBN }},
                    { _desc, new AttributeValue { S = model.Description }}
                }

            };
            var response = await AmazonDynamoDBClient.PutItemAsync(request);
            if(response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("Failed to create new book.");
            }
            return model;
        }

        public async Task<List<Book>> GetAll()
        {
            var response = (await AmazonDynamoDBClient.ScanAsync(new ScanRequest(_tableName)));
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException("Failed to get all items from a table.");
            }
            return response.Items
                .Select(i =>
                new Book() 
                {
                    ISBN = i[_isbn].S,
                    Description = i[_desc].S,
                    Title = i[_title].S
                })
                .ToList();
        }

        public async Task<Book> Get(string id)
        {
            var request = new GetItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>() { { _isbn, new AttributeValue { S = id } } },
            };
            var response = await AmazonDynamoDBClient.GetItemAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Failed to get an items {id} from a table.");
            }

            return new Book()
            { 
                Description = response.Item[_desc].S,
                ISBN = response.Item[_isbn].S,
                Title = response.Item[_title].S
            };
        }

        public async Task<Book> Update(Book model)
        {
            var request = new UpdateItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>() { { _isbn, new AttributeValue { S = model.ISBN } } },
                ExpressionAttributeNames = new Dictionary<string, string>()
                {
                    {"#T", _title},
                    {"#D", _desc},
                },
                ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                {
                    {":title",new AttributeValue { S = model.Title}},
                    {":desc",new AttributeValue {S = model.Description}},
                },
                UpdateExpression = "SET #T = :title, #D = :desc"
            };
            var response = await AmazonDynamoDBClient.UpdateItemAsync(request);
            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Failed to update item {model.ISBN} from a table.");
            }
            return model;
        }

        public async Task Delete(string id)
        {
            var request = new DeleteItemRequest
            {
                TableName = _tableName,
                Key = new Dictionary<string, AttributeValue>() { { _isbn, new AttributeValue { S = id } } },
            };

            var response = await AmazonDynamoDBClient.DeleteItemAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new ApplicationException($"Failed to delete item {id} from a table.");
            }
        }
    }
}
