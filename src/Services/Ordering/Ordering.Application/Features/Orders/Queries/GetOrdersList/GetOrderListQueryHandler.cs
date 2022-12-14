using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        public GetOrderListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepo = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<List<OrdersVm>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var orderlist = await _orderRepo.GetOrderByUserName(request.UserName);
            return _mapper.Map<List<OrdersVm>>(orderlist);
             
        }
    }
}
