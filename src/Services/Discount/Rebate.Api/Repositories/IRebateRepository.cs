using Rebate.Api.Models;

namespace Rebate.Api.Repositories
{
    public interface IRebateRepository
    {
        Task<Coupon> GetRebate(string productName);
        Task<bool> CreateRebate(Coupon coupon);
        Task<bool> DeleteRebate(string productName);
        Task<bool> UpdateRebate(Coupon coupon);
    }
}
