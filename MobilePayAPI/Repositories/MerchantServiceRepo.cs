
using MobilePayAPI.Data;
using MobilePayAPI.Entities;
using MobilePayAPI.Interfaces;
using NLog;
using ILogger = NLog.ILogger;
using Microsoft.EntityFrameworkCore;

namespace MobilePayAPI.Repositories
{
    public class MerchantServiceRepo : IMerchantService
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public MerchantServiceRepo(AppDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Merchant> CreateMerchant(List<Merchant> merchants)
        {
            if (merchants == null)
            {
                throw new ArgumentNullException(nameof(merchants));
            }
            foreach (var merhant in merchants)
            {
                _context.Merchants.Add(merhant);
                _context.SaveChanges(); 
            }

            return (IEnumerable<Merchant>)merchants;
        }

        public async Task<Merchant> GetMerchantAsync(Guid id)
        {
            _logger.Info("****Get Merchant By Id Method Start****");
            try
            {
                if(id== Guid.Empty)
                {
                    throw new ArgumentNullException(nameof(id));
                }
                return await _context.Merchants.FirstOrDefaultAsync(x => x.ID == id);
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error($"ArgumentNullException {id} does not exist or " + ex.StackTrace); 
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return default(Merchant);
        }

        public IEnumerable<Merchant> GetMerchants()
        {
            _logger.Info("****Get All Merchant Method Start****");
            try
            {
                var merchantItems = _context.Merchants.ToList();
                return merchantItems;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error($"Argument Null Exception Merchants Method Issue getting a list" + ex.StackTrace);
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return Enumerable.Empty<Merchant>();
        }

        public IEnumerable<Merchant> GetMerchantTransactionsByName(string merchantName)
        {
            _logger.Info("****Get Merchant Transactions By Name Method Start****");
            try
            {
                var merchantItems = _context.Merchants.Where(m => m.MerchantName.Equals(merchantName)).ToList();
                var fees = merchantItems.ToList().Where(a => a.Amount != 0).Select(x => x.Amount).Sum();
                return merchantItems;
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error($"Argument Null Exception Get Merchant Transactions By Name issue getting a list by name" + ex.StackTrace);
                
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return Enumerable.Empty<Merchant>();
        }

        public async Task<bool> SaveChangesAsync()
        {
            _logger.Info("****Save Changes Method Start****");
            try
            {
                return (await _context.SaveChangesAsync() > 0);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return default(bool);
        }
    }
}