using GrpcKopikoDemoServer.Data;
using GrpcKopikoDemoServer.DataModels;
using Microsoft.EntityFrameworkCore;

namespace GrpcKopikoDemoServer.DataModelServices.OrderModelService
{
	public class OrderService : IOrderService
	{
		private readonly DatabaseContext _context;

		public OrderService(DatabaseContext context) 
		{
			_context = context;
		}

		async Task<Order> IOrderService.Create(Order order)
		{
			try
			{
				await _context.Orders.AddAsync(order);
				await _context.SaveChangesAsync();
				return order;
			}
			catch (Exception)
			{
				throw;
			}
		}

		async Task<List<Order>> IOrderService.GetAllOrdersByUserID(long userId)
		{
			try
			{
				var orders = await _context.Orders.Where(x => x.UserID == userId).Include(x => x.OrderDetails).ThenInclude(x => x.product).ToListAsync();
				return orders;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
