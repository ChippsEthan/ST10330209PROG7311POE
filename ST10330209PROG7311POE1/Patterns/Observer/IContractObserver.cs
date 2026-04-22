using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.Patterns.Observer
{
    public interface IContractObserver
    {
        void Update(Contract contract, string oldStatus, string newStatus);
    }
}