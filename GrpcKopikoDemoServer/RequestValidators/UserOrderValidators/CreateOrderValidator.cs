using FluentValidation;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.RequestValidators.UserOrderValidators
{
	public class CreateOrderValidator : AbstractValidator<CreateNewOrderRequest>
	{
		public CreateOrderValidator() 
		{
			RuleFor(request => request.OrderDetailsList).NotEmpty().WithMessage("Order details list is required.");
			RuleFor(request => request.OrderShipName).NotEmpty().WithMessage("Order ship name is required.");
			RuleFor(request => request.OrderShipAddress).NotEmpty().WithMessage("Order ship address is required.");
		}
	}
}
