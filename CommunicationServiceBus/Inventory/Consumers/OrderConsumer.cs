using Azure.Messaging.ServiceBus;
using Inventory.Data;
using Inventory.Models;
using System.Text.Json;

namespace Inventory.Consumers
{
    public class OrderConsumer : IOrderConsumer
    {
        ServiceBusClient _serviceBusClient;
        IConfiguration _configuration;
        ServiceBusProcessor _processor;
        public OrderConsumer(IConfiguration configuration)
        {
            _configuration = configuration;
            _serviceBusClient = new ServiceBusClient(_configuration["ServiceBus:Connection"]);
            _processor = _serviceBusClient.CreateProcessor(_configuration["ServiceBus:Queue"], new ServiceBusProcessorOptions());
        }

        public async Task RegisterReceiveMessageHandler()
        {
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;

            await _processor.StartProcessingAsync();
        }

        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();

            Order message = JsonSerializer.Deserialize<Order>(body);
            MyData.Data.Add(message);

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}