using AutoMapper;
using Basket.Api.Entities;
using ErrorBus.Messages.Events;

namespace Basket.Api.Mapper
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
        }
    }
}
