using MobilePayAPI.Entities;
using MobilePayAPI.Interfaces;
using MobilePayAPI.Services;
using System;
using Xunit;

namespace TestMobilePayApi
{
    public class UnitTestFeeService
    {
        FeeService feeService = new FeeService();
        [Fact]
        public void TestGetChargeRateMethod()
        {        
            // Arrange
            decimal expected = 250000;
            // Act
            decimal actual = feeService.GetChargeRate(25000000);
            // Asset
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestIsWeekendMethod()
        {
            // Arrange
            bool expected = false;
            // Act
            bool actual = feeService.IsWeekend();
            // Asset
            Assert.Equal(expected, actual);
        }
        
        [Fact]
        public void TestGetMerchantDiscountRateMethod()
        {
            // Arrange
            decimal expected = 62500;
            // Act
            decimal actual = feeService.GetMerchantDiscountRate("Tesla", 250000);
            // Asset
            Assert.Equal(expected, actual);
        }
        
            [Fact]
        public void TestIsMonthExceed10TransactionsMethod()
        {
            // Arrange
            bool expected = true;
            // Act
            bool actual = feeService.IsMonthExceed10Transactions(11);
            // Asset
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestGetVolumeDiscountMethod()
        {
            // Arrange
            decimal expected = 37500;
            // Act
            decimal actual = feeService.GetVolumeDiscount(250000, 62500);
            // Asset
            Assert.Equal(expected, actual);
        }
        [Fact]
        public void TestGetGetTotalFeeMethod()
        {
            // Arrange
            decimal expected = 150000;
            // Act
            decimal actual = feeService.GetTotalFee(250000, 62500, 37500);
            // Asset
            Assert.Equal(expected, actual);
        }
    }
}