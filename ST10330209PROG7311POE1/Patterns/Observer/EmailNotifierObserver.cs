using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.Patterns.Observer
{
    public class EmailNotifierObserver : IContractObserver
    {
        public void Update(Contract contract, string oldStatus, string newStatus)
        {
            
            Console.WriteLine($"[OBSERVER] Contract {contract.Id} changed from {oldStatus} to {newStatus}. Email sent.");
        }
    }
}