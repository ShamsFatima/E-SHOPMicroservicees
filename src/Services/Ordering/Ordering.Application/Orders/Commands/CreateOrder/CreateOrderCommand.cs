using BuildingBlocks.CQRS;
using FluentValidation;
using Ordering.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ordering.Application.Orders.Commands.CreateOrder;
public record CreateOrderCommand(OrderDto OrderDto)
    :ICommand<CreateOrderResult>;
public record CreateOrderResult(Guid Id);
public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x=>x.OrderDto.OrderName).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.OrderDto.CustomerId).NotEmpty().WithMessage("Customer Id is required");
        RuleFor(x => x.OrderDto.OrderItems).NotEmpty().WithMessage("Order Items should not be less than 0");
    }
}
