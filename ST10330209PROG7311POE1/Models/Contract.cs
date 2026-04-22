using System.ComponentModel.DataAnnotations;

namespace ST10330209PROG7311POE1.Models
{
    public class Contract
    {
        public int Id { get; set; }

        [Required]
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        public string Status { get; set; } = "Draft"; 

        public string ServiceLevel { get; set; } = string.Empty;

        
        public string? SignedAgreementPath { get; set; }

        public ICollection<ServiceRequest>? ServiceRequests { get; set; }


        public enum ContractType { Standard, Premium, Enterprise }

        
        public ContractType Type { get; set; } = ContractType.Standard;
    }
}
