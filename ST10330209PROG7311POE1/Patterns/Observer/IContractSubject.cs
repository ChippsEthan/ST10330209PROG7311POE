using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.Patterns.Observer
{
    public interface IContractSubject
    {
        void Attach(IContractObserver observer);
        void Detach(IContractObserver observer);
        void NotifyObservers(Contract contract, string oldStatus, string newStatus);
    }
}