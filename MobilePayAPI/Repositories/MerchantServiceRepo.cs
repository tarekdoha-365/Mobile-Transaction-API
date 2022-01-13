
using MobilePayAPI.Data;
using MobilePayAPI.Entities;
using MobilePayAPI.Interfaces;
using NLog;
using ILogger = NLog.ILogger;

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
        public Merchant CreateMerchant(Merchant merchant)
        {
            _logger.Info("****Create Merchant Method Start****");
            try
            {
                Console.WriteLine("CreateMerchant Method Start");
                _context.Merchants.Add(merchant);
                _context.SaveChanges();
                return merchant;
            }
            catch (NotSupportedException ex)
            {
                _logger.Error($"Not Supported Exception {merchant.Amount} or {merchant.MerchantName} do not have value!" + ex.StackTrace);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return default(Merchant);
        }

        public Merchant GetMerchant(Guid id)
        {
            _logger.Info("****Get Merchant By Id Method Start****");
            try
            {
                var merchantItem = _context.Merchants.FirstOrDefault(x => x.ID == id);
                return merchantItem;
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

        public bool SaveChanges()
        {
            _logger.Info("****Save Changes Method Start****");
            try
            {
                return (_context.SaveChanges() > 0);
            }
            catch (Exception ex)
            {
                _logger.Error(ex.StackTrace);
            }
            return default(bool);
        }
    }
}