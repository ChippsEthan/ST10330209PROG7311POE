using ST10330209PROG7311POE1.Models;

namespace ST10330209PROG7311POE1.Patterns.Factory
{
    public interface IContractFactory
    {
        Contract CreateContract(int clientId, DateTime startDate, DateTime endDate, string status, string serviceLevel);
    }
}