using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.ObjectModels;

namespace GrpcKopikoDemoServer.DataModelServices.UserModelService
{
	public interface IUserService
	{
		public Task<User> Create(User user);
		public Task<User> Get(long userId);
		public Task<User> GetUserByEmailAddress (string emailAddress);
		public Task<User> SetUserRefreshToken(User user, RefreshTokenModel refreshToken);
		public Task<User> RevokeRefrshToken(long userId);		
	}
}
