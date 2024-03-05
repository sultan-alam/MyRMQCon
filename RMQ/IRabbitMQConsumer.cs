/*	
================================================================
Title:		IRabbitMQConsumer
Author:		Sultan     
Purpose:	RabbitMQ Consumer Interface
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
    public interface IRabbitMQConsumer
    {
        /// <summary>
        /// Receiving message from the Queue (FIFO)
        /// </summary>        
        public void ReceiveMessage(Func<string, bool> callback);
    }
}
