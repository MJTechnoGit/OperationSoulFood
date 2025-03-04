using Azure.Messaging.ServiceBus;
using Azure.Identity;
using OperationSoulFood.Services.EmailAPI.Messaging.Interfaces;
using OperationSoulFood.Services.EmailAPI.Models.Dto;
using System.Text;
using Newtonsoft.Json;

namespace OperationSoulFood.Services.EmailAPI.Messaging
{    

    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly IConfiguration _configuration;

        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessorOptions _emailCartProcessorOptions;

        public AzureServiceBusConsumer(IConfiguration configuration)
        {
            _configuration = configuration;

            serviceBusConnectionString = _configuration.GetConnectionString("ServiceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessorOptions = new ServiceBusProcessorOptions() { AutoCompleteMessages = false };
            _emailCartProcessor = client.CreateProcessor(emailCartQueue, _emailCartProcessorOptions);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            await _emailCartProcessor.StartProcessingAsync();
        }

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();
        }


        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            // This is where you will receive the message.
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                // TODO - try to log the email.
               await args.CompleteMessageAsync(args.Message);
               
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

    }
}
