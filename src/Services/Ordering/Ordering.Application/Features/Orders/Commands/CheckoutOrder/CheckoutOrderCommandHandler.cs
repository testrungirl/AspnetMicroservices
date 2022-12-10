using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderrepo;
        private readonly IMapper _mapper;
        private IEmailService _emailService;
        private ILogger<CheckoutOrderCommandHandler> _logger;

        public CheckoutOrderCommandHandler(IOrderRepository orderrepo, IMapper mapper, IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderrepo = orderrepo ?? throw new ArgumentNullException(nameof(orderrepo));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _orderrepo.AddAsync(orderEntity);

            _logger.LogInformation($"Order {newOrder.Id} is successfully created.");
            _logger.LogInformation("Order {0} is successfully created.", newOrder.Id);

            await sendMail(newOrder);

            return newOrder.Id;
        }

        private async Task sendMail(Order order)
        {
            var email = new Email()
            {
                To = "tomeg.gmail.com",
                Body = "Order was created.",
                Subject = "Order Creation Status"
            };
            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Order {0} failed due to an error with the mail service: {1}", order.Id, ex.Message);
            }
        }
    }
}
