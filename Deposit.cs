/*	
================================================================
Title:		Deposit
Author:		Sultan     
Purpose:	POCO class for simulate transaction
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
    public class Deposit
    {
        public int TransactionId { get; set; }
        public decimal TransactionAmount { get; set; }
        public DateTime TransactionDate { get; set; }

        public Deposit(int transactionId, decimal transcationAmount, DateTime transactionDate)
        {
            TransactionId = transactionId;
            TransactionAmount = transcationAmount;
            TransactionDate = transactionDate;
        }
    }
}