using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;

        public UpdateOrderCommandHandler(IOrderRepository orderRepo, IMapper mapper, ILogger<UpdateOrderCommandHandler> logger)
        {
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        async Task<Unit> IRequestHandler<UpdateOrderCommand, Unit>.Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var orderObj = await _orderRepo.GetByIdAsync(request.Id);
            if (orderObj == null)
            {
                _logger.LogError("Order does not exist in database");
                return Unit.Value;
            }
            //var orderEntity = _mapper.Map<Order>(request);
            //await _orderRepo.UpdateAsync(orderEntity);
             _mapper.Map(request, orderObj, typeof(UpdateOrderCommand), typeof(Order));

            await _orderRepo.UpdateAsync(orderObj);

            return Unit.Value;
        }
    }
}
