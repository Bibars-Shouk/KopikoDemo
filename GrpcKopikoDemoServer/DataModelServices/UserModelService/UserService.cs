using GrpcKopikoDemoServer.Data;
using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.ObjectModels;
using Microsoft.EntityFrameworkCore;


namespace GrpcKopikoDemoServer.DataModelServices.UserModelService
{
	public class UserService : IUserService
	{
		private readonly DatabaseContext _context;
		public UserService(DatabaseContext context)
		{
			_context = context;
			
		}

		async Task<User> IUserService.Create(User user)
		{
			try
			{
				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();
				return user;
			}
			catch (Exception)
			{
				throw;
			}
		}

		async Task<User> IUserService.Get(long userId)
		{
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x => x.UserID == userId);
				return user;
			}
			catch (Exception)
			{

				throw;
			}
		}

		async Task<User> IUserService.GetUserByEmailAddress(string emailAddress)
		{
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(x=> x.Email == emailAddress);
				return user;
			}
			catch (Exception)
			{
				throw;
			}
		}
		
		async Task<User> IUserService.SetUserRefreshToken(User user, RefreshTokenModel refreshToken)
		{
			try
			{
				var userToUpdate = await _context.Users.FirstAsync(x => x.UserID == user.UserID);
				userToUpdate.RefreshToken = refreshToken.RefreshToken;
				userToUpdate.RefreshTokenExpiryDate = refreshToken.RefreshTokenExpiryDate;
				await _context.SaveChangesAsync();

				return userToUpdate;
			}	
			catch (Exception)
			{
				throw;
			}
		}

		async Task<User> IUserService.RevokeRefrshToken(long userId)
		{
			try
			{
				var userToUpdate = await _context.Users.FirstAsync(x => x.UserID == userId);
				userToUpdate.RefreshToken = "0";
				await _context.SaveChangesAsync();

				return userToUpdate;
			}
			catch (Exception)
			{
				throw;
			}
		}		

		//async Task<bool> IUserService.Delete(long userId)
		//{
		//	try
		//	{
		//		var user = await _context.Users.FirstAsync(x => x.UserID == userId);
		//		_context.Users.Remove(user);
		//		var deleted = await _context.SaveChangesAsync();
		//		return deleted > 0;
		//	}
		//	catch (Exception)
		//	{
		//		throw;
		//	}
		//}
	}
}
