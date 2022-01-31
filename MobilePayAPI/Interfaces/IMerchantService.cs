

using MobilePayAPI.Entities;

namespace MobilePayAPI.Interfaces
{
    public interface IMerchantService
    {
        Task<bool> SaveChangesAsync();
        #region GetMerchants
        Task<IEnumerable<Merchant>> GetMerchantsAsync();
        #endregion
        Task<IEnumerable<Merchant>> GetMerchantTransactionsByNameAsync(string merchantName);
        #region GetMerchantAsync
        Task<Merchant> GetMerchantAsync(Guid id);
        #endregion
        #region CreateMerchant
        IEnumerable<Merchant> CreateMerchant(List<Merchant> merchants);
        #endregion
    }
}