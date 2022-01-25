# MobilePay-Code-Assignment

* .Net 6 Framework Core Webapi VS-2022.
* Insomnia Json File Exported.
* Packages Used (Hangfire, EntityFrameWork SQL Server, DependencyInjection, Design, NLog).
* Create DbContext and Migrate Entity Merchant to SQL Server Local using cli commands (Add-Update).
* Add HangFire Service Setup to the Program File.
* Create the Repository of Merchant and Interface Class.
* Create Post Create Item Merchant from the Repository and controller to write to Sql Server.
* Create the Controller with enqueue WebApi endpoint, To Get Single Transaction and List of Transactions.
* Get a list of Created Items in Merchant Table by the Merchant Name.
* Create a Fee Discount and Charge Calculation, IsWeekend? IsmoreThan10TransactionMonthly Service and its interface.
* Create a Unit Test Project Targetting Windows 10, to test feeService.
* Create Try Catch with Logging.
* Query Info groubed by Merchant name as a Key and aggregate all transaction Values.
### Query used to verify Sum() Amounts :
{
Select MerchantName, COUNT(MerchantName),Sum (Amount) as Total_Fee FROM [MerchantDb].[dbo].[Merchants] Group By MerchantName
}

![alt text](https://github.com/tarekdoha-365/MobilePay/blob/master/MobilePayAPI/Images/MobilePayResult.PNG)



* To Do Add Swagger, Async... 