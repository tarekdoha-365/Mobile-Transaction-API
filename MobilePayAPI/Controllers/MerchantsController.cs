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
        public async Task<ActionResult<MerchantReadDto>> CreateMerchant(List<MerchantCreateDto> merchantCreateDto)
        {
            var merchantModel = _mapper.Map<List<Merchant>>(merchantCreateDto);
            _merchantService.CreateMerchant(merchantModel);
            await _merchantService.SaveChangesAsync();
            var merchantReadDto = _mapper.Map<List<MerchantReadDto>>(merchantModel);
            return CreatedAtRoute(
                routeName: "GetMerchant", 
                routeValues: new {id= merchantModel.Any()}, 
                value: merchantReadDto);
        }
        [HttpGet]
        public ActionResult<IEnumerable<MerchantReadDto>> GetMerchants()
        {
            var merchanatItems = _merchantService.GetMerchants();
            return Ok(_mapper.Map<IEnumerable<MerchantReadDto>>(merchanatItems));
        }
        [HttpGet("GetMerchant/{id}", Name = "GetMerchant")]
        public async Task<ActionResult<MerchantReadDto>> GetMerchant(Guid id)
        {
            var merchantItem = await _merchantService.GetMerchantAsync(id);
            if (merchantItem == null)
            {
                return NotFound();
            }
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
                }
                else
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.20) * charge;
                    decimal Volume = (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                }
                if (merchantName == "Rema1000" && merchanatItems.Count() > 9 && DateTime.Now! > expiryDate)
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.15) * charge;
                    decimal Volume = Convert.ToDecimal(0.20) * (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                }
                else
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.15) * charge;
                    decimal Volume = (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                }
                if (merchantName == "McDonald" && merchanatItems.Count() > 9 && DateTime.Now! > expiryDate)
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.05) * charge;
                    decimal Volume = Convert.ToDecimal(0.20) * (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
                }
                else
                {
                    decimal charge = Convert.ToDecimal(0.01) * total;
                    decimal discount = Convert.ToDecimal(0.05) * charge;
                    decimal Volume = (charge - discount);
                    decimal Totalfee = charge - discount - Volume;
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