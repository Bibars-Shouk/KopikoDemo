using AutoMapper;
using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.Profiles.ProductProfile
{
	public class ProductMapper : Profile
	{
		public ProductMapper() 
		{
			CreateMap<Product, ProductDetails>().ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => $"https://fakeimg.pl/800x800/?text={GetShortName(src.ProductName)}&font=Roboto" ));
		}

		public static string GetShortName(string name)
		{
			return name.Split(" ")[2];
		}
	}
}
