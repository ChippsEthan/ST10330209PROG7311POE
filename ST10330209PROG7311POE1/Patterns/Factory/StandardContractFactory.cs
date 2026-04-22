using ST10330209PROG7311POE1.Models;
using static ST10330209PROG7311POE1.Models.Contract;

namespace ST10330209PROG7311POE1.Patterns.Factory
{
    public class StandardContractFactory : IContractFactory
    {
        public Contract CreateContract(int clientId, DateTime startDate, DateTime endDate, string status, string serviceLevel)
        {
            return new Contract
            {
                ClientId = clientId,
                StartDate = startDate,
                EndDate = endDate,
                Status = status,
                ServiceLevel = serviceLevel,
                Type = ContractType.Standard
            };
        }
    }
}