using Rebate.Api.Data;
using Rebate.Api.Models;

namespace Rebate.Api.Repositories
{
    public interface IRebateRepository
    {
        Task<GenericResponse<Coupon>> GetRebate(string productName);
        Task<GenericResponse<bool>> CreateRebate(Coupon coupon);
        Task<GenericResponse<bool>> DeleteRebate(string productName);
        Task<GenericResponse<bool>> UpdateRebate(Coupon coupon);
    }
}
