using GrpcKopikoDemoServer.ObjectModels;
using System.Security.Claims;

namespace GrpcKopikoDemoServer.Interfaces
{
	public interface ITokenServiceHelper
	{
		AccessTokenModel GenerateAccessToken(IEnumerable<Claim> claims);
		RefreshTokenModel GenerateRefreshToken();
		ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);
		ClaimsPrincipal GetPrincipalFromValidToken(string accessToken);
	}
}
