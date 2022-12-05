using Rebate.GRPC.Protos;

namespace Basket.Api.GrpcServices
{
    public class RebateGrpcService
    {
        private readonly RebateProtoService.RebateProtoServiceClient _rebateProtoService;

        public RebateGrpcService(RebateProtoService.RebateProtoServiceClient rebateProtoService)
        {
            Console.WriteLine("GRPC Client Instantiation");
            _rebateProtoService = rebateProtoService;
        }

        public async Task<CouponModel> GetRebate(string productName)
        {
            var rebateRequest = new ProductName { Name = productName };
            return await _rebateProtoService.GetDiscountAsync(rebateRequest);
        }
    }
}
