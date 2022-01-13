using System.ComponentModel.DataAnnotations;

namespace MobilePayAPI.Entities
{
    public class Merchant 
    {
        [Key]
        public Guid ID { get; set; }
        [Required]
        public string MerchantName {get; set; }
        [Required]
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        }
}