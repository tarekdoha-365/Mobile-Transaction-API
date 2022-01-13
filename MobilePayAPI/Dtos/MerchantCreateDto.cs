using System.ComponentModel.DataAnnotations;
using MobilePayAPI.Entities;

namespace MobilePayAPI.Dtos
{
    public class MerchantCreateDto
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string MerchantName {get; set; }
        public decimal Amount { get; set; }        
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}