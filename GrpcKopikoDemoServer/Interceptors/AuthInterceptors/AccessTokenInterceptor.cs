using Grpc.Core;
using Grpc.Core.Interceptors;
using GrpcKopikoDemoServer.Interfaces;
using System.Security.Claims;

namespace GrpcKopikoDemoServer.Interceptors.AuthInterceptors
{
	public class AccessTokenInterceptor : Interceptor
	{

		private readonly ITokenServiceHelper _tokenServiceHelper;
		public AccessTokenInterceptor(ITokenServiceHelper tokenServiceHelper)
		{
			_tokenServiceHelper = tokenServiceHelper;
		}

		public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
		{
			try
			{
				string accessToken = context.RequestHeaders.FirstOrDefault(h => h.Key == "authorization").Value.Split(" ").Last();

				var principal = _tokenServiceHelper.GetPrincipalFromValidToken(accessToken);
				if (principal != null)
				{
					var claimsIdentity = (ClaimsIdentity)principal.Identity;
					var httpContext = context.GetHttpContext();
					httpContext.User.AddIdentity(claimsIdentity);
					return base.UnaryServerHandler(request, context, continuation);
				}

				throw new UnauthorizedAccessException("Token has expired. Please obtain a new token and try again.");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}
	}
}
