using Azure.Messaging.ServiceBus;
using Azure.Identity;
using OperationSoulFood.Services.EmailAPI.Messaging.Interfaces;
using OperationSoulFood.Services.EmailAPI.Models.Dto;
using System.Text;
using Newtonsoft.Json;
using OperationSoulFood.Services.EmailAPI.Services;

namespace OperationSoulFood.Services.EmailAPI.Messaging
{    

    public class AzureServiceBusConsumer : IAzureServiceBusConsumer
    {
        private readonly string serviceBusConnectionString;
        private readonly string emailCartQueue;
        private readonly string registerUserQueue;
        private readonly IConfiguration _configuration;

        private readonly EmailService _emailService;

        private ServiceBusProcessor _emailCartProcessor;
        private ServiceBusProcessor _registerUserProcessor;

        private ServiceBusProcessorOptions _emailCartProcessorOptions;

        public AzureServiceBusConsumer(IConfiguration configuration, EmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;

            serviceBusConnectionString = _configuration.GetConnectionString("ServiceBusConnectionString");

            emailCartQueue = _configuration.GetValue<string>("TopicAndQueueNames:EmailShoppingCartQueue");
            registerUserQueue = _configuration.GetValue<string>("TopicAndQueueNames:RegisterUserQueue");

            var client = new ServiceBusClient(serviceBusConnectionString);
            _emailCartProcessorOptions = new ServiceBusProcessorOptions() { AutoCompleteMessages = false };
            _emailCartProcessor = client.CreateProcessor(emailCartQueue, _emailCartProcessorOptions);

            _registerUserProcessor = client.CreateProcessor(registerUserQueue);
        }

        public async Task Start()
        {
            _emailCartProcessor.ProcessMessageAsync += OnEmailCartRequestReceived;
            _emailCartProcessor.ProcessErrorAsync += ErrorHandler;

            await _emailCartProcessor.StartProcessingAsync();


            _registerUserProcessor.ProcessMessageAsync += OnUserRegisterRequestReceived;
            _registerUserProcessor.ProcessErrorAsync += ErrorHandler;

            await _emailCartProcessor.StartProcessingAsync();
        }

       

        public async Task Stop()
        {
            await _emailCartProcessor.StopProcessingAsync();
            await _emailCartProcessor.DisposeAsync();

            await _registerUserProcessor.StopProcessingAsync();
            await _registerUserProcessor.DisposeAsync();
        }


        private async Task OnEmailCartRequestReceived(ProcessMessageEventArgs args)
        {
            // This is where you will receive the message.
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            CartDto objMessage = JsonConvert.DeserializeObject<CartDto>(body);

            try
            {
                // Log the email.
                await _emailService.EmailCartAndLog(objMessage);

               // This will remove messages from the queue so you won't be able to view them with this code enabled.
               // To view messages comment out the line of code below.
               await args.CompleteMessageAsync(args.Message);
               
                
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task OnUserRegisterRequestReceived(ProcessMessageEventArgs args)
        {
            // This is where you will receive the message.
            var message = args.Message;
            var body = Encoding.UTF8.GetString(message.Body);

            string email = JsonConvert.DeserializeObject<string>(body);

            try
            {
                // Log the email.
                await _emailService.RegisterUserEmailAndLog(email);

                // This will remove messages from the queue so you won't be able to view them with this code enabled.
                // To view messages comment out the line of code below.
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
