using GrpcKopikoDemoServer.Interfaces;
using GrpcKopikoDemoServer.ObjectModels;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GrpcKopikoDemoServer.ServicesHelpers
{
	public class TokenServiceHelper : ITokenServiceHelper
	{
		private readonly string JWT_SECRET = Environment.GetEnvironmentVariable("JWT_SECRET");
		private readonly int ACCESS_TOKEN_EXPIRATION_MINS = int.Parse(Environment.GetEnvironmentVariable("ACCESS_TOKEN_EXPIRATION_MINS"));
		private readonly int REFRESH_TOKEN_EXPIRATION_DAYS = int.Parse(Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRATION_DAYS"));

		public AccessTokenModel GenerateAccessToken(IEnumerable<Claim> claims)
		{
			var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
			var tokenSecurityKey = Encoding.UTF8.GetBytes(JWT_SECRET);
			var tokenExpiryDateTime = DateTime.Now.AddMinutes(ACCESS_TOKEN_EXPIRATION_MINS);

			var securityTokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new System.Security.Claims.ClaimsIdentity(claims),
				Expires = tokenExpiryDateTime,
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenSecurityKey), SecurityAlgorithms.HmacSha256Signature)
			};

			var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
			var tokenString = jwtSecurityTokenHandler.WriteToken(securityToken);

			var accessToken = new AccessTokenModel
			{
				AccessToken = tokenString,
				AccessTokenExpiryDate = tokenExpiryDateTime,
			};

			return accessToken;
		}

		public RefreshTokenModel GenerateRefreshToken()
		{
			var tokenString = Guid.NewGuid().ToString();
			var tokenExpiryDateTime = DateTime.Now.AddDays(REFRESH_TOKEN_EXPIRATION_DAYS);
			var refreshToken = new RefreshTokenModel
			{
				RefreshToken = tokenString,
				RefreshTokenExpiryDate = tokenExpiryDateTime,
			};

			return refreshToken;
		}

		public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = false,
				ValidateIssuer = false,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWT_SECRET)),
				ValidateLifetime = false
			};
			var tokenHandler = new JwtSecurityTokenHandler();
			SecurityToken securityToken;
			var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);
			var jwtSecurityToken = securityToken as JwtSecurityToken;
			if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid token");
			return principal;
		}

		public ClaimsPrincipal GetPrincipalFromValidToken(string accessToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(JWT_SECRET);
			try
			{
				var claimsPrincipal = tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
				{
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuerSigningKey = true,
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
				}, out var validatedToken);
				var principal = claimsPrincipal;

				return principal;
			}
			catch (Exception ex)
			{
				throw;
			}
		}
	}
}
