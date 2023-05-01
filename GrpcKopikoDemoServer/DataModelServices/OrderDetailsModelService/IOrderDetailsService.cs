using GrpcKopikoDemoServer.DataModels;

namespace GrpcKopikoDemoServer.DataModelServices.OrderDetailsService
{
	public interface IOrderDetailsService
	{
		public Task<OrderDetails> Create(OrderDetails orderDetails);
		public Task<List<OrderDetails>> CreateRange(List<OrderDetails> lstOrderDetails);
		public Task<List<OrderDetails>> GetAllOrderDetailsByUserId(long userId);
	}
}
