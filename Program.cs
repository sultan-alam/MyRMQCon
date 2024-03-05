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
                    services.AddSingleton<IRabbitMQConsumer, RabbitMQConsumer>(i => new RabbitMQConsumer(config["RMQ:HostName"], config["RMQ:Port"], config["RMQ:UserName"], config["RMQ:Password"], config["RMQ:Exchange"], config["RMQ:Type"], config["RMQ:Queue"], config["RMQ:FetchSize"]));
                })
                .Build()
                .Run();
        }
    }
}
