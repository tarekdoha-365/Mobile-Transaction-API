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

            var merchanatItems = _merchantService.GetMerchantTransactionsByName(merchantName);
            var MerchantTotalFees = from merchant in merchanatItems
                                    group merchant by merchant.MerchantName into merchantGroup
                                    select new
                                    {
                                        MerchantName = merchantGroup.Key,
                                        Amount = merchantGroup.Sum(x => x.Amount),
                                    };
            if (MerchantTotalFees == null)
            {
                return NotFound();
            }
            return Ok(MerchantTotalFees);
        }
    }
}