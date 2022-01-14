using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using MobilePayAPI.Dtos;
using MobilePayAPI.Entities;
using MobilePayAPI.Interfaces;
using System.Web.Http.Description;

namespace MobilePayAPI.Contrllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMerchantService _merchantService;
        private readonly IFeeService _feeService;
        private readonly IMapper _mapper;
        private readonly IBackgroundJobClient _backgroundJobClient;
        public decimal total = 0;
        public MerchantsController(IMerchantService merchantService,
            IMapper mapper,
            IFeeService feeService,
            IBackgroundJobClient backgroundJobClient)
        {
            _merchantService = merchantService;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;

        }
        [HttpPost]
        public ActionResult<MerchantReadDto> CreateMerchant(MerchantCreateDto merchantCreateDto)
        {
            var merchantModel = _mapper.Map<Merchant>(merchantCreateDto);
            _merchantService.CreateMerchant(merchantModel);
            _merchantService.SaveChanges();
            var merchantReadDto = _mapper.Map<MerchantReadDto>(merchantModel);
            return Ok(merchantReadDto);
        }
        [HttpGet]
        public ActionResult<IEnumerable<MerchantReadDto>> GetMerchants()
        {
            var merchanatItems = _merchantService.GetMerchants();
            foreach (var merchant in merchanatItems)
            {
                _backgroundJobClient.Enqueue(() => GetMerchants());
                _backgroundJobClient.Enqueue(() => 
                Console.WriteLine($"Merchant Items Enqueue : " +
                $"Merchant ID:{0}, Metrchant Name:{1}, Merchant Amount: {2}, Merchant TimeSpan: {3}"
                ,merchant.ID,merchant.MerchantName, merchant.Amount, merchant.Timestamp));
            }
            
            return Ok(_mapper.Map<IEnumerable<MerchantReadDto>>(merchanatItems));
        }
        [HttpGet("MerchantGuid/{id}", Name = "GetMerchant")]
        public ActionResult<MerchantReadDto> GetMerchant(Guid id)
        {
            var merchantItem = _merchantService.GetMerchant(id);
            _backgroundJobClient.Enqueue(() => _merchantService.GetMerchant(id));
            return Ok(_mapper.Map<MerchantReadDto>(merchantItem));
        }

        [HttpGet("{merchantName}", Name = "GetMerchantTransactionsByName")]
        public ActionResult<IEnumerable<MerchantReadDto>> GetMerchantsByMerchantName(string merchantName)
        {
            var merchanatItems = _merchantService.GetMerchantTransactionsByName(merchantName);
            return Ok(_mapper.Map<IEnumerable<MerchantReadDto>>(merchanatItems));
        }

        [HttpGet("{merchantName}/QueryTransactionsByMerchantName", Name = "GetQueryFeeTransactionByMarchantName")]
        public async Task<ActionResult> GetQueryFeeInfo(string merchantName)
        {
            string dayOfweek = new DateTime().DayOfWeek.ToString();
            string dateInString = "01.01.2020";
            DateTime startDate = DateTime.Parse(dateInString);
            DateTime expiryDate = startDate.AddDays(30);
            var merchanatItems = _merchantService.GetMerchantTransactionsByName(merchantName);
            var MerchantTotalFees = from merchant in merchanatItems
                                    group merchant by merchant.MerchantName into merchantGroup
                                    select new
                                    {
                                        MerchantName = merchantGroup.Key,
                                        Amount = merchantGroup.Sum(x => x.Amount),
                                    };
            foreach (var merchant in merchanatItems)
            {
                for (int i = 0; i < MerchantTotalFees.Count(); i++)
                {
                    total += merchant.Amount;
                }
            }

            if ((dayOfweek != "Saturday") || (dayOfweek != "Sunday"))
            {
                if (merchantName == "Tesla" && merchanatItems.Count() > 9 && DateTime.Now! > expiryDate)
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.25) * charge;
                    decimal Volume = Convert.ToDecimal(0.20) * (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                    RecurringJob.AddOrUpdate(() => Console.WriteLine($"Tesla Exceed 10 Transactions monthly,The description bill is:" + $"1% fee charge of {total} DKK = {charge}, 25% Tesla discount of {total} DKK = {discount},Transaction is during a Friday, so the fee will not be free,20% volume discount of {charge} - {discount} = {Volume}, Total fee = {total}DKK - {charge}DKK - {Volume}DKK - {discount}DKK = {Totalfee} DKK ", total, charge, discount, Volume, Totalfee), Cron.Monthly);
                }
                else
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.20) * charge;
                    decimal Volume = (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                    RecurringJob.AddOrUpdate(() => Console.WriteLine($"Tesla not Exceed 10 Transactions monthly,The description bill is:" + $"1% fee charge of {total} DKK = {charge}, 25% Tesla discount of {total} DKK = {discount},Transaction is during a Friday, so the fee will not be free,{charge} - {discount} = {Volume}, Total fee = {total}DKK - {charge}DKK - {Volume}DKK - {discount}DKK = {Totalfee} DKK ", total, charge, discount, Volume, Totalfee), Cron.Monthly);
                }
                if (merchantName == "Rema1000" && merchanatItems.Count() > 9 && DateTime.Now! > expiryDate)
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.15) * charge;
                    decimal Volume = Convert.ToDecimal(0.20) * (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                    RecurringJob.AddOrUpdate(() => Console.WriteLine($"Rema1000 Exceed 10 Transactions monthly,The description bill is:" + $"1% fee charge of {total} DKK = {charge}, 25% Tesla discount of {total} DKK = {discount},Transaction is during a Friday, so the fee will not be free,20% volume discount of {charge} - {discount} = {Volume}, Total fee = {total}DKK - {charge}DKK - {Volume}DKK - {discount}DKK = {Totalfee} DKK ", total, charge, discount, Volume, Totalfee), Cron.Monthly);
                }
                else
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.15) * charge;
                    decimal Volume = (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                    RecurringJob.AddOrUpdate(() => Console.WriteLine($"Rema1000 not Exceed 10 Transactions monthly,The description bill is:" + $"1% fee charge of {total} DKK = {charge}, 25% Tesla discount of {total} DKK = {discount},Transaction is during a Friday, so the fee will not be free,{charge} - {discount} = {Volume}, Total fee = {total}DKK - {charge}DKK - {Volume}DKK - {discount}DKK = {Totalfee} DKK ", total, charge, discount, Volume, Totalfee), Cron.Monthly);
                }
                if (merchantName == "McDonald" && merchanatItems.Count() > 9 && DateTime.Now! > expiryDate)
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.05) * charge;
                    decimal Volume = Convert.ToDecimal(0.20) * (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                    RecurringJob.AddOrUpdate(() => Console.WriteLine($"McDonald Exceed 10 Transactions monthly,The description bill is:" + $"1% fee charge of {total} DKK = {charge}, 25% Tesla discount of {total} DKK = {discount},Transaction is during a Friday, so the fee will not be free,20% volume discount of {charge} - {discount} = {Volume}, Total fee = {total}DKK - {charge}DKK - {Volume}DKK - {discount}DKK = {Totalfee} DKK ", total, charge, discount, Volume, Totalfee), Cron.Monthly);
                }
                else
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.05) * charge;
                    decimal Volume = (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                    RecurringJob.AddOrUpdate(() => Console.WriteLine($"McDonald not Exceed 10 Transactions monthly,The description bill is:" + $"1% fee charge of {total} DKK = {charge}, 25% Tesla discount of {total} DKK = {discount},Transaction is during a Friday, so the fee will not be free,{charge} - {discount} = {Volume}, Total fee = {total}DKK - {charge}DKK - {Volume}DKK - {discount}DKK = {Totalfee} DKK ", total, charge, discount, Volume, Totalfee), Cron.Monthly);
                }
            }

            if (MerchantTotalFees == null)
            {
                return NotFound();
            }
            return Ok(MerchantTotalFees);
        }
    }
}