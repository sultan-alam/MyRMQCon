using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyRMQCon.RMQ;

/*	
================================================================
Title:		Program
Author:		Sultan     
Purpose:	Main method, starting the Consumer as Hosted Service
Creation:	02-Mar-2024
================================================================
Modification History    
Author		Date		Description of change    
Sultan      11-Mar-24   Sending configuration to RabbitMQ Consumer
================================================================    
Missing:    

================================================================    
*/

namespace MyRMQCon
{
    public class Program
    {
        static void Main(string[] args)
        {
            Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    var config = context.Configuration;
                    // Host
                    services.AddHostedService<ConsumerHost>();
                    // RabbitMQ
                    services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>(i => new RabbitMQConsumer(config));
                })
                .Build()
                .Run();
        }
    }
}
