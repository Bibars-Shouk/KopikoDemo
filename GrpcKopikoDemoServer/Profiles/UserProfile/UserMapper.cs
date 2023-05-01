using AutoMapper;
using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.ObjectModels;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.RequestMaps.AuthMaps
{
	public class UserMapper : Profile
	{
		public UserMapper() 
		{ 
			CreateMap<RegisterRequest, User>();
			CreateMap<RefreshTokenModel, User>();
		}
	}
}
