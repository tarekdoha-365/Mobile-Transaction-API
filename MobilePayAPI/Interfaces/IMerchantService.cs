

using MobilePayAPI.Entities;

namespace MobilePayAPI.Interfaces
{
    public interface IMerchantService
    {
        bool SaveChanges();
        IEnumerable<Merchant> GetMerchants();
        IEnumerable<Merchant> GetMerchantTransactionsByName(string merchantName);
        Merchant GetMerchant(Guid id);
        IEnumerable<Merchant> CreateMerchant(List<Merchant> merchants);
    }
}