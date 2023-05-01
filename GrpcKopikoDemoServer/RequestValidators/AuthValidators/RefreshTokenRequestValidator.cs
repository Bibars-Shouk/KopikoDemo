using FluentValidation;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.RequestValidators.AuthValidators
{
	public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
	{
		public RefreshTokenRequestValidator() 
		{
			RuleFor(request => request.RefreshToken).NotEmpty().WithMessage("Refresh token is required.");
			RuleFor(request => request.AccessToken).NotEmpty().WithMessage("Access token is required.");
		}
	}
}
