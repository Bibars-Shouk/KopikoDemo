using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcKopikoDemoServer.DataModels;
using GrpcKopikoDemoServer.DataModelServices.OrderDetailsService;
using GrpcKopikoDemoServer.DataModelServices.OrderModelService;
using GrpcKopikoDemoServer.Protos;
using Microsoft.AspNetCore.Authorization;

namespace GrpcKopikoDemoServer.Services
{
	[Authorize]
	public class UserOrderService : UserOrder.UserOrderBase
	{
		private readonly IOrderService _orderService;
		private readonly IOrderDetailsService _orderDetailsService;
		private readonly IMapper _mapper;

		public UserOrderService(IOrderService orderService, IOrderDetailsService orderDetailsService, IMapper mapper)
		{
			_orderService = orderService;
			_orderDetailsService = orderDetailsService;
			_mapper = mapper;
		}

		public override async Task<CreateNewOrderReply> CreateNewOrder(CreateNewOrderRequest request, ServerCallContext context)
		{
			double totalOrderPrice = 0;
			var userId = long.Parse(context.GetHttpContext().User.FindFirst("userId").Value);
			var requestedProductsList = request.OrderDetailsList;
			var orderNumber = Guid.NewGuid().ToString();
			requestedProductsList.ToList().ForEach(product =>
			{
				totalOrderPrice += (product.Quantity * product.PricePerPiece);
			});
			request.OrderNumber = orderNumber;
			request.OrderTotalPrice = totalOrderPrice;
			request.UserID = userId;

			Order objOrder = _mapper.Map<Order>(request);
			var newOrder = await _orderService.Create(objOrder);


			requestedProductsList.ToList().ForEach(product =>
			{
				product.OrderID = newOrder.OrderID;
				product.OrderNumber = orderNumber;
				product.UserID = userId;
			});
			List<OrderDetails> lstObjOrderDetails = _mapper.Map<List<OrderDetails>>(requestedProductsList);
			await _orderDetailsService.CreateRange(lstObjOrderDetails);

			var reply = new CreateNewOrderReply { OrderSaved = true };

			return reply;
		}

		public override async Task<GetUserOrdersReply> GetUserOrders(Empty request, ServerCallContext context)
		{
			var userId = long.Parse(context.GetHttpContext().User.FindFirst("userId").Value);
			var userOrdersList = await _orderService.GetAllOrdersByUserID(userId);
			List<UserOrderItem> y = _mapper.Map<List<UserOrderItem>>(userOrdersList);
			var reply = new GetUserOrdersReply();
			reply.Orders.AddRange(y);
			return reply;
		}
	}
}
