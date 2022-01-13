using MobilePayAPI.Dtos;
using MobilePayAPI.Entities;

namespace MobilePayAPI.Interfaces
{
    public interface IFeeService
    {
        decimal GetMerchantDiscountRate(string MerchantName, decimal dicountOf);
        decimal GetChargeRate(decimal Amount);
        bool IsWeekend();
        bool IsMonthExceed10Transactions(int TransactionCount);
        decimal GetVolumeDiscount(decimal chargeRate, decimal discount);
        decimal GetTotalFee(decimal chargeRate, decimal discount, decimal volume);
    }
}
