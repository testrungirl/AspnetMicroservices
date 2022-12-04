using Rebate.GRPC.Data;
using Rebate.GRPC.Models;

namespace Rebate.GRPC.Repositories
{
    public interface IRebateRepository
    {
        Task<GenericResponse<Coupon>> GetRebate(string productName);
        Task<GenericResponse<bool>> CreateRebate(Coupon coupon);
        Task<GenericResponse<bool>> DeleteRebate(string productName);
        Task<GenericResponse<bool>> UpdateRebate(Coupon coupon);
    }
}
