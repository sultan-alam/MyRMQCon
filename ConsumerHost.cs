using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MyRMQCon.RMQ;
using Newtonsoft.Json;

/*	
================================================================
Title:		ConsumerHost
Author:		Sultan     
Purpose:	Host Service to run the consumer in background
Creation:	02-Mar-2024
================================================================
Modification History    
Author		Date		Description of change    

================================================================    
Missing:    

================================================================    
*/
namespace MyRMQCon
{
    public class ConsumerHost : IHostedService
    {
        private IRabbitMQConsumer _rabbitMQConsumer;
        public ConsumerHost(IConfiguration config, IRabbitMQConsumer consumer)
        {            
            _rabbitMQConsumer = consumer;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _rabbitMQConsumer.ReceiveMessage(ProcessMessage);
            return Task.CompletedTask;            
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public bool ProcessMessage(string message)
        {
            Deposit deposit = JsonConvert.DeserializeObject<Deposit>(message);            
            if (deposit != null)
                return true;
            return false;
        }
    }
}
