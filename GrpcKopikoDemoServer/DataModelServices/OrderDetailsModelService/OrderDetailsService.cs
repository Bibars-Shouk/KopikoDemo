using GrpcKopikoDemoServer.Data;
using GrpcKopikoDemoServer.DataModels;
using Microsoft.EntityFrameworkCore;

namespace GrpcKopikoDemoServer.DataModelServices.OrderDetailsService
{
	public class OrderDetailsService : IOrderDetailsService
	{
		private readonly DatabaseContext _context;

		public OrderDetailsService(DatabaseContext context) 
		{
			_context = context;
		}

		async Task<OrderDetails> IOrderDetailsService.Create(OrderDetails orderDetails)
		{	
			await _context.OrderDetails.AddAsync(orderDetails);
			await _context.SaveChangesAsync();
			return orderDetails;
		}


		async Task<List<OrderDetails>> IOrderDetailsService.CreateRange(List<OrderDetails> orderDetails)
		{
			await _context.OrderDetails.AddRangeAsync(orderDetails);
			await _context.SaveChangesAsync();
			return orderDetails;
		}


		async Task<List<OrderDetails>> IOrderDetailsService.GetAllOrderDetailsByUserId(long userId)
		{
			var orderDetailsList = await _context.OrderDetails.Where(x => x.UserID == userId).Include(x => x.order).Include(x => x.product).ToListAsync();

			return orderDetailsList;
		}
	}
}
