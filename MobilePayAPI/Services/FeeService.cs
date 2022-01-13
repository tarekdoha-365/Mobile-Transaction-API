using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Runtime.Caching;
using Hangfire;
using MobilePayAPI.Data;
using MobilePayAPI.Dtos;
using MobilePayAPI.Entities;
using MobilePayAPI.Interfaces;
using Newtonsoft.Json.Linq;
using NLog;
using ILogger = NLog.ILogger;

namespace MobilePayAPI.Services
{
    public class FeeService : IFeeService
    {
        private readonly IMerchantService _merchantService;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly ObjectCache _cache = MemoryCache.Default;
        private readonly AppDbContext _context;
        public FeeService()
        {

        }
        public FeeService(IMerchantService merchantService, AppDbContext context)
        {
            _merchantService = merchantService;
            _context = context;
        }

        public decimal GetChargeRate(decimal amount)
        {
            if (IsWeekend().Equals(false))
            {
                return Convert.ToDecimal(0.01) * amount;
            }
            return 1;
        }
        public bool IsWeekend()
        {
            string dayOfweek = new DateTime().DayOfWeek.ToString();
            if ((dayOfweek == "Saturday") || (dayOfweek == "Sunday"))
            {
                return true;
            }
            return false;
        }
        public decimal GetMerchantDiscountRate(string MerchantName, decimal discountOf)
        {
            switch (MerchantName)
            {
                case "Tesla":
                    return Convert.ToDecimal(0.25) * discountOf;
                    break;
                case " Rema1000":
                    return Convert.ToDecimal(0.15) * discountOf;
                    break;
                case " McDonald":
                    return Convert.ToDecimal(0.05) * discountOf;
                    break;
                default:
                    Console.WriteLine("No Discount Found");
                    break;
                    Console.WriteLine($"Discount Occured Merchant Name{0} with Amount" +
                        $"{1}", MerchantName, discountOf);
            }
            return default(decimal);
        }

        public bool IsMonthExceed10Transactions(int TransactionCount)
        {
            int NumberOfTransactionPerMonth = (int)new DateTime().Month;
            if (TransactionCount > 9)
            {
                return true;
            }
            return false;
        }

        public decimal GetVolumeDiscount(decimal chargeRate, decimal discount)
        {
            return Convert.ToDecimal(0.20) * (chargeRate - discount);
        }

        public decimal GetTotalFee(decimal chargeRate, decimal discount, decimal volume)
        {
            return (chargeRate - discount - volume);
        }
    }
}