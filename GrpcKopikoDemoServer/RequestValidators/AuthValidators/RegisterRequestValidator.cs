using FluentValidation;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.RequestValidators.AuthValidators
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
		public RegisterRequestValidator() 
		{
			RuleFor(request => request.FirstName).NotEmpty().WithMessage("First name is required.");
			RuleFor(request => request.LastName).NotEmpty().WithMessage("Last name is required.");
			RuleFor(request => request.Email).EmailAddress().WithMessage("Invalid Email Address.");
			RuleFor(request => request.Password).NotEmpty().WithMessage("Password is required.");
			RuleFor(request => request.Password).MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
		}
	}
}
