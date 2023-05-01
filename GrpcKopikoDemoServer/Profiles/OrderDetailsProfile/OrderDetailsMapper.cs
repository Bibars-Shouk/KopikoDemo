using AutoMapper;
using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.RequestMaps.OrderDetailsProfile
{
	public class OrderDetailsMapper : Profile
	{
        public OrderDetailsMapper()
        {
            CreateMap<OrderDetailsRequest, OrderDetails>()
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => (src.Quantity * src.PricePerPiece)));
        }

    }
}
