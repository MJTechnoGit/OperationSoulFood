using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Azure.Identity;

using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace SoulFood.MessageBus
{
    public class MessageBus : IMessageBus
    {

        private IConfiguration _configuration;
        private string connectionString;

        public MessageBus(IConfiguration configuration)
        {
            _configuration = configuration;
            
            connectionString = "Endpoint=sb://operationsoulweb.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=XP3CkHcA+7TW2cTBvLE3oTvu1DvXokQa0+ASbEQQwVM=;EntityPath=emailshoppingcart";
        }

        public async Task PublishMessage(object message, string topic_queue_Name)
        {
            await using var client = new ServiceBusClient(connectionString);
            ServiceBusSender sender = client.CreateSender(topic_queue_Name);

            try
            {
                var jsonMessage = JsonConvert.SerializeObject(message);
                ServiceBusMessage finalMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(jsonMessage))
                {
                    CorrelationId = Guid.NewGuid().ToString(),
                    ContentType = "application/json"                    
                };               

                await sender.SendMessageAsync(finalMessage);
            }
            catch (ServiceBusException ex)
            { 
            
            }
            catch(Exception ex)
            {

            }
            finally
            {
                //await sender.DisposeAsync();
                //await client.DisposeAsync();            
            } 
        }
    }
}
