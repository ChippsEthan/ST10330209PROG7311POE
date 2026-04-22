using System.ComponentModel.DataAnnotations;

namespace ST10330209PROG7311POE1.Models
{
    public class ServiceRequest
    {
        public int Id { get; set; }

        [Required]
        public int ContractId { get; set; }
        public Contract? Contract { get; set; }

        public string Description { get; set; } = string.Empty;
        public decimal CostInUSD { get; set; }
        public decimal CostInZAR { get; set; }
        public string Status { get; set; } = "Open";
    }
}
