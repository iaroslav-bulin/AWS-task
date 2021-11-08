using Amazon.SQS;
using Amazon.SQS.Model;
using System;
using System.Threading.Tasks;

namespace SqsLogger
{
    public class SqsService : IDisposable
    {
        private readonly String _queueUrl;
        private readonly IAmazonSQS _sqsClient;

        public SqsService(string queueUrl, IAmazonSQS sqsClient)
        {
            _queueUrl = queueUrl;
            _sqsClient = sqsClient;
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
