using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

/*	
================================================================
Title:		RabbitMQConsumer
Author:		Sultan     
Purpose:	RabbitMQ Consumer/ Subscriber
Creation:	02-Mar-2024
================================================================
Modification History    
Author		Date		Description of change    

================================================================    
Missing:    

================================================================    
*/

namespace MyRMQCon.RMQ
{
    public class RabbitMQConsumer : IDisposable, IRabbitMQConsumer
    {
        private string HostName = string.Empty;
        private int Port = 0;
        private string UserName = string.Empty;
        private string Password = string.Empty;
        private string Exchange = string.Empty;
        private string Type = string.Empty;
        private string Queue = string.Empty;
        private ushort FetchSize = 0;

        private IConnection _connection;
        private IModel _channel;
        private bool disposedValue;

        /// <summary>
        /// Ctor receives the connection parameter
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="port"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public RabbitMQConsumer(string hostName, string port, string userName, string password, string exchange, string type, string queue, string fetchSize)
        {
            HostName = hostName;
            Port = Convert.ToInt32(port);
            UserName = userName;
            Password = password;
            Exchange = exchange;
            Type = type;
            Queue = queue;
            FetchSize = Convert.ToUInt16(fetchSize);
        }
        public void ReceiveMessage(Func<string, bool> callback)
        {
            //Rabbit MQ Server
            /*var factory = new ConnectionFactory
            { 
                Uri = new Uri("amqp://guest:guest@localhost:5672")
            }*/
            var factory = new ConnectionFactory
            {
                HostName = HostName,
                Port = Port,
                UserName = UserName,
                Password = Password
            };
            //Create the RabbitMQ connection
            _connection = factory.CreateConnection();
            //Creating channel with session and model            
            _channel = _connection.CreateModel();
            //Declaring Exchange
            _channel.ExchangeDeclare(Exchange, Type, durable: true, autoDelete: false);
            //Declaring the queue
            _channel.QueueDeclare(Queue, durable: true, exclusive: false, autoDelete: false);
            //Binding Queue to Exchange
            _channel.QueueBind(Queue, Exchange, string.Empty);
            //The first Param prefetchSize must be 0 which is the only implementation RabbitMQ.Client currently have
            _channel.BasicQos(0, FetchSize, false);

            //Setting event object which listen message from chanel which is sent by producer
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (sender, eventArgs) =>
            {
                Console.WriteLine("==============================================================================================");
                var body = eventArgs.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message received: {message} at {DateTime.Now}");
                bool success = callback.Invoke(message);
                if (success)
                {
                    //Will acknowledge on success only :: auto acknowledgement must be false on channel
                    _channel.BasicAck(eventArgs.DeliveryTag, false);
                    Console.WriteLine($"Message Acknowledged.");
                    Thread.Sleep(1000);
                }
            };
            //We are not auto acknowledging (autoAck), rather on successfull cosnume we'll do that on each Invoke            
            _channel.BasicConsume(queue: Queue, autoAck: false, consumer: consumer);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _channel.Close();
                    _connection.Close();
                }                
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
