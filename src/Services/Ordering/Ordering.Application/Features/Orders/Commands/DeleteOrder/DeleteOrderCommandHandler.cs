using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly ILogger<DeleteOrderCommandHandler> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepo;

        public DeleteOrderCommandHandler(ILogger<DeleteOrderCommandHandler> logger, IMapper mapper, IOrderRepository orderRepo)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _orderRepo = orderRepo ?? throw new ArgumentNullException(nameof(orderRepo));
        }

        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var OrderObj = await _orderRepo.GetByIdAsync(request.Id);
            if (OrderObj == null)
            {
                _logger.LogError("Order does not exist on database");
                return Unit.Value;
            }

            await _orderRepo.DeleteAsync(OrderObj);
            return Unit.Value;
        }
    }
}
