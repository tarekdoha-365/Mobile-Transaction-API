using System.ComponentModel.DataAnnotations;
using MobilePayAPI.Entities;

namespace MobilePayAPI.Dtos
{
    public class MerchantReadDto
    {
        public Guid ID { get; set; }
        public string MerchantName {get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}