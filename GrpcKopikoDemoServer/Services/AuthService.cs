using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.DataModelServices.UserModelService;
using GrpcKopikoDemoServer.Extensions.MapperExtension;
using GrpcKopikoDemoServer.Interfaces;
using GrpcKopikoDemoServer.Protos;
using System.Security.Claims;

namespace GrpcKopikoDemoServer.Services
{
	public class AuthService : Auth.AuthBase
	{
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly ITokenServiceHelper _tokenServiceHelper;
		public AuthService(IMapper mapper, IUserService userService, ITokenServiceHelper tokenServiceHelper)
		{
			_mapper = mapper;
			_userService = userService;
			_tokenServiceHelper = tokenServiceHelper;
		}

		public override async Task<AuthReply> Register(RegisterRequest request, ServerCallContext context)
		{
			var existingUser = await _userService.GetUserByEmailAddress(request.Email);
			if (existingUser != null)
				throw new RpcException(new Status(StatusCode.AlreadyExists, "Email address is already in use"));

			var refreshToken = _tokenServiceHelper.GenerateRefreshToken();

			var user = _mapper.Map<RegisterRequest, User>(request).Map(refreshToken, _mapper);

			var addedUser = await _userService.Create(user);

			var userClame = new Claim("userId", addedUser.UserID.ToString());
			var claims = new List<Claim> { userClame };

			var accessToken = _tokenServiceHelper.GenerateAccessToken(claims.AsEnumerable());

			var reply = new AuthReply
			{
				AccessToken = accessToken.AccessToken,
				AccessTokenExpiryDate = Timestamp.FromDateTimeOffset(accessToken.AccessTokenExpiryDate),
				RefreshToken = refreshToken.RefreshToken,
				RefreshTokenExpiryDate = Timestamp.FromDateTimeOffset(refreshToken.RefreshTokenExpiryDate)
			};

			return reply;
		}

		public override async Task<AuthReply> RefreshToken(RefreshTokenRequest request, ServerCallContext context)
		{
			if (request.RefreshToken == "0")
				throw new RpcException(new Status(StatusCode.PermissionDenied, "Invalid Request"));

			string accessToken = request.AccessToken;
			var principal = _tokenServiceHelper.GetPrincipalFromExpiredToken(accessToken);
			var claimsIdentity = (ClaimsIdentity)principal.Identity;
			var userId = claimsIdentity.FindFirst("userId").Value;

			var user = await _userService.Get(long.Parse(userId));

			if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryDate <= DateTime.Now)
				throw new RpcException(new Status(StatusCode.PermissionDenied, "Invalid Request"));

			var newRefreshToken = _tokenServiceHelper.GenerateRefreshToken();

			user = await _userService.SetUserRefreshToken(user, newRefreshToken);

			var userClame = new Claim("userId", user.UserID.ToString());
			var claims = new List<Claim> { userClame };

			var newAccessToken = _tokenServiceHelper.GenerateAccessToken(claims.AsEnumerable());

			var reply = new AuthReply
			{
				AccessToken = newAccessToken.AccessToken,
				AccessTokenExpiryDate = Timestamp.FromDateTimeOffset(newAccessToken.AccessTokenExpiryDate),
				RefreshToken = newRefreshToken.RefreshToken,
				RefreshTokenExpiryDate = Timestamp.FromDateTimeOffset(newRefreshToken.RefreshTokenExpiryDate)
			};

			return reply;
		}

		public override async Task<AuthReply> Login(LoginRequest request, ServerCallContext context)
		{
			var existingUser = await _userService.GetUserByEmailAddress(request.Email);
			if (existingUser == null || existingUser.Password != request.Password)
				throw new RpcException(new Status(StatusCode.NotFound, "Invalid Credentials"));

			var refreshToken = _tokenServiceHelper.GenerateRefreshToken();

			existingUser = await _userService.SetUserRefreshToken(existingUser, refreshToken);

			var userClame = new Claim("userId", existingUser.UserID.ToString());
			var claims = new List<Claim> { userClame };

			var accessToken = _tokenServiceHelper.GenerateAccessToken(claims.AsEnumerable());

			var reply = new AuthReply
			{
				AccessToken = accessToken.AccessToken,
				AccessTokenExpiryDate = Timestamp.FromDateTimeOffset(accessToken.AccessTokenExpiryDate),
				RefreshToken = refreshToken.RefreshToken,
				RefreshTokenExpiryDate = Timestamp.FromDateTimeOffset(refreshToken.RefreshTokenExpiryDate)
			};

			return reply;
		}
	}
}
