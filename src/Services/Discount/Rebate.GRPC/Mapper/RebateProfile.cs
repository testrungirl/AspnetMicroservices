using AutoMapper;
using Rebate.GRPC.Models;
using Rebate.GRPC.Protos;

namespace Rebate.GRPC.Mapper
{
    public class RebateProfile: Profile
    {
        public RebateProfile()
        {
            CreateMap<Coupon, CouponModel>().ReverseMap();
        }
    }
}
