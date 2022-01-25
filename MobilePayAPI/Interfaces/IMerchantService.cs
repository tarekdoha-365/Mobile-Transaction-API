

using MobilePayAPI.Entities;

namespace MobilePayAPI.Interfaces
{
    public interface IMerchantService
    {
        Task<bool> SaveChangesAsync();
        IEnumerable<Merchant> GetMerchants();
        IEnumerable<Merchant> GetMerchantTransactionsByName(string merchantName);
        #region GetMerchantAsync
        Task<Merchant> GetMerchantAsync(Guid id);
        #endregion
        #region CreateMerchant
        IEnumerable<Merchant> CreateMerchant(List<Merchant> merchants);
        #endregion
    }
}