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
        private readonly IRecurringJobManager _recurringJobManager;
        public decimal total = 0;
        public MerchantsController(IMerchantService merchantService,
            IMapper mapper,
            IFeeService feeService,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager)
        {
            _merchantService = merchantService;
            _mapper = mapper;
            _backgroundJobClient = backgroundJobClient;
            _recurringJobManager = recurringJobManager;
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
            _backgroundJobClient.Enqueue(() => _merchantService.GetMerchants());
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

                    if ((dayOfweek != "Saturday") || (dayOfweek != "Sunday"))
                    {
                        if (merchantName == "Tesla" && MerchantTotalFees.Count() > 10 && DateTime.Now! > expiryDate)
                        {
                            decimal charge = Convert.ToDecimal(0.01) * total;
                            decimal discount = Convert.ToDecimal(0.20) * charge;
                            decimal contain10TransactionVolume = Convert.ToDecimal(0.20) * (charge - discount);
                            decimal Totalfee = charge - discount - contain10TransactionVolume;
                        }
                        else
                        {
                            decimal charge = Convert.ToDecimal(0.01) * total;
                            decimal discount = Convert.ToDecimal(0.20) * charge;
                            decimal contain10TransactionVolume = /*Convert.ToDecimal(0.20) **/ (charge - discount);
                            decimal Totalfee = charge - discount - contain10TransactionVolume;
                        }
                        if (merchantName == "Rema1000" && MerchantTotalFees.Count() > 10 && DateTime.Now! > expiryDate)
                        {
                            decimal charge = Convert.ToDecimal(0.01) * total;
                            decimal discount = Convert.ToDecimal(0.15) * charge;
                            decimal contain10TransactionVolume = Convert.ToDecimal(0.20) * (charge - discount);
                            decimal Totalfee = charge - discount - contain10TransactionVolume;
                        }
                        else
                        {
                            decimal charge = Convert.ToDecimal(0.01) * total;
                            decimal discount = Convert.ToDecimal(0.15) * charge;
                            decimal contain10TransactionVolume = /*Convert.ToDecimal(0.20) **/ (charge - discount);
                            decimal Totalfee = charge - discount - contain10TransactionVolume;
                        }
                        if (merchantName == "McDonald" && MerchantTotalFees.Count() > 10 && DateTime.Now! > expiryDate)
                        {
                            decimal charge = Convert.ToDecimal(0.01) * total;
                            decimal discount = Convert.ToDecimal(0.05) * charge;
                            decimal contain10TransactionVolume = Convert.ToDecimal(0.20) * (charge - discount);
                            decimal Totalfee = charge - discount - contain10TransactionVolume;
                        }
                        else
                        {
                            decimal charge = Convert.ToDecimal(0.01) * total;
                            decimal discount = Convert.ToDecimal(0.05) * charge;
                            decimal contain10TransactionVolume = /*Convert.ToDecimal(0.20) **/ (charge - discount);
                            decimal Totalfee = charge - discount - contain10TransactionVolume;
                        }
                    }
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