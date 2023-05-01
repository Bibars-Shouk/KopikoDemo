using GrpcKopikoDemoServer.DataModels;

namespace GrpcKopikoDemoServer.DataModelServices.OrderModelService
{
	public interface IOrderService
	{
		public Task<Order> Create(Order order);
		public Task<List<Order>> GetAllOrdersByUserID(long userId);
	}
}
