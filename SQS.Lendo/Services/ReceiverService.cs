using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SQS.Lendo.Services
{
    public class ReceiverService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Lendo as mensagens");
            var credential = new BasicAWSCredentials("AKIATQA5AYNZU4HY3PTV", "jvAWZlfV1BtYrYGQMyspJgrbypyY7svNZHmgkCcU");
            var client = new AmazonSQSClient(credentials: credential, region: Amazon.RegionEndpoint.SAEast1);
            while (!stoppingToken.IsCancellationRequested)
            {
                var request = new ReceiveMessageRequest()
                {
                    QueueUrl = "https://sqs.sa-east-1.amazonaws.com/240579036019/TesteEnvioFila",
                    WaitTimeSeconds = 50,
                    VisibilityTimeout = 10//cada vez que ele lê, ele nao disponibiliza para outros serviços lerem
                };
                var response = await client.ReceiveMessageAsync(request);
                foreach (var message in response.Messages)
                {
                    Console.WriteLine(message);
                    if (message.Body.Contains("Exception")) continue;
                    await client.DeleteMessageAsync("https://sqs.sa-east-1.amazonaws.com/240579036019/TesteEnvioFila", message.ReceiptHandle);
                }
            }
        }
    }
}