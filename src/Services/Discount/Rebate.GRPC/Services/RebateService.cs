using AutoMapper;
using Grpc.Core;
using Rebate.GRPC.Models;
using Rebate.GRPC.Protos;
using Rebate.GRPC.Repositories;

namespace Rebate.GRPC.Services
{
    public class RebateService : RebateProtoService.RebateProtoServiceBase
    {
        private readonly IRebateRepository _rebateRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RebateService> _logger;

        public RebateService(IRebateRepository rebateRepository, ILogger<RebateService> logger)
        {
            _rebateRepository = rebateRepository ?? throw new ArgumentNullException(nameof(rebateRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task<CouponModel> GetCoupon(ProductName request, ServerCallContext context)
        {
            if(request.Name == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Rebate with ProductName is required"));
            }
            var coupondata = await _rebateRepository.GetRebate(request.Name);
            if (coupondata.Success)
            {
                _logger.LogInformation("Rebate is retrieved for ProductName: {0}, Amount : {1}", coupondata.Data.ProductName, coupondata.Data.Amount);
                return _mapper.Map<CouponModel>(coupondata.Data);
            }
            throw new RpcException(new Status(StatusCode.NotFound, "Rebate with ProductName does not exists"));
        }
        public override async Task<CouponModel> CreateCoupon(CData request, ServerCallContext context)
        {
            if (request== null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Rebate Object is required"));
            }
            var obj =  _mapper.Map<Coupon>(request.Coupon);
            var coupondata = await _rebateRepository.CreateRebate(obj);
            if (coupondata.Success)
            {
                _logger.LogInformation("New Rebate object created successfully");
                return _mapper.Map<CouponModel>(coupondata.Data);
            }
            throw new RpcException(new Status(StatusCode.Internal, "An erroer occured"));
        }
        public override async Task<StatusMessage> DeleteCoupon(ProductName request, ServerCallContext context)
        {
            if (request.Name == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Rebate with ProductName is required"));
            }
            var coupondata = await _rebateRepository.DeleteRebate(request.Name);
            if (coupondata.Success)
            {
                _logger.LogInformation("Rebate has been deleted successfully!");
                var res = new StatusMessage();
                res.Status = "Rebate has been deleted successfully!";
                return res;
            }
            throw new RpcException(new Status(StatusCode.NotFound, "Rebate with ProductName does not exists"));
        }
        public override async Task<CouponModel> EditCoupon(CData request, ServerCallContext context)
        {
            if (request == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Rebate Object is required"));
            }
            var obj = _mapper.Map<Coupon>(request.Coupon);
            var coupondata = await _rebateRepository.UpdateRebate(obj);
            if (coupondata.Success)
            {
                _logger.LogInformation("Rebate has been updated successfully");
                return _mapper.Map<CouponModel>(coupondata.Data);
            }
            throw new RpcException(new Status(StatusCode.NotFound, "Rebate with ProductName does not exists"));
        }
    }
}
