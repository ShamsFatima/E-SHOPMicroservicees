using BuildingBlocks.CQRS;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Ordering.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderCommand(Guid Id):ICommand<DeleteOrderResult>;  
public record DeleteOrderResult(bool IsSuccess);
public class DeleteCommandValidator:AbstractValidator<DeleteOrderCommand>
{
    public DeleteCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Order Id is required");
    }
}   

