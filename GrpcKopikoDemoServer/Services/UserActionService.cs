using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcKopikoDemoServer.DataModelServices.UserModelService;
using GrpcKopikoDemoServer.Protos;
using Microsoft.AspNetCore.Authorization;

namespace GrpcKopikoDemoServer.Services
{
	[Authorize]
	public class UserActionService : UserAction.UserActionBase
	{
		private readonly IUserService _userService;

		public UserActionService(IUserService userService)
		{
			_userService = userService;
		}

		public override async Task<GetLoggedInUserReply> GetLoggedInUser(Empty request, ServerCallContext context)
		{
			var userId = context.GetHttpContext().User.FindFirst("userId").Value;
            var user = await _userService.Get(long.Parse(userId));

			var reply = new GetLoggedInUserReply 
			{
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
			};

			return reply;
		}

		public override async Task<Empty> LogoutUser(Empty request, ServerCallContext context)
		{
			var userId = context.GetHttpContext().User.FindFirst("userId").Value;
		    await _userService.RevokeRefrshToken(long.Parse(userId));

			return new Empty();			
		}
	}
}
