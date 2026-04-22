using System.ComponentModel.DataAnnotations;

namespace ST10330209PROG7311POE1.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string ContactDetails { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        public ICollection<Contract>? Contracts { get; set; }
    }
}
    