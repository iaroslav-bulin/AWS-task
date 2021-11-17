using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

namespace DynamoDBApi.SqsLogger
{
    public class SqsService : IDisposable
    {
        private readonly string _queueUrl;
        private readonly IAmazonSQS _sqsClient;

        public SqsService(string queueName, IAmazonSQS sqsClient)
        {
            _sqsClient = sqsClient;
            var queueUrlResp = sqsClient.GetQueueUrlAsync(queueName).Result;
            _queueUrl = queueUrlResp.QueueUrl;
        }

        public async Task<SendMessageResponse> SendMessage(string messageBody)
        {
              return await _sqsClient.SendMessageAsync(_queueUrl, messageBody);
        }

        public void Dispose()
        {
            _sqsClient.Dispose();
        }
    }
}
