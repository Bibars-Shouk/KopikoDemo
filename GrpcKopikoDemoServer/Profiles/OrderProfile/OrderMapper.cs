using AutoMapper;
using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.RequestMaps.OrderProfile
{
	public class OrderMapper : Profile
	{
        public OrderMapper()
        {
            CreateMap<CreateNewOrderRequest, Order>();
        }
    }
}
