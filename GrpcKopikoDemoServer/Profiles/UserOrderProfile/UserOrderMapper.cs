using AutoMapper;
using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.Profiles.ProductProfile;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.Profiles.UserOrderProfile
{
	public class UserOrderMapper : Profile
	{
		public UserOrderMapper() 
		{
			CreateMap<Order, UserOrderItem>();
			CreateMap<OrderDetails, UserOrderDetailsItem>();
			CreateMap<Product, UserProductItem>().ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => $"https://fakeimg.pl/800x800/?text={ProductMapper.GetShortName(src.ProductName)}&font=Roboto"));
		}
	}
}
