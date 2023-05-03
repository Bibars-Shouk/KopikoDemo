using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcKopikoDemoServer.DataModelServices.ProductModelService;
using GrpcKopikoDemoServer.Protos;

namespace GrpcKopikoDemoServer.Services
{
	public class ProductItemService : ProductItem.ProductItemBase
	{		
		private readonly IProductService _productService;
		private readonly IMapper _mapper;

		public ProductItemService(IProductService productService, IMapper mapper)
		{			
			_productService = productService;
			_mapper = mapper;
		}

		public override async Task<GetAllProductsReply> GetAllProducts(Empty request, ServerCallContext context)
		{
			var productsList = await _productService.GetAll();
			List<ProductDetails> productsReplyList = _mapper.Map<List<ProductDetails>>(productsList);			
			var reply = new GetAllProductsReply();
			reply.Products.AddRange(productsReplyList);
			return await Task.FromResult(reply);
		}

        public override async Task<ProductDetails> GetProductById(GetProductByIdRequest request, ServerCallContext context)
        {
			var product = await _productService.GetById(request.ProductID);
			var reply = _mapper.Map<ProductDetails>(product);
			return reply;
        }
    }
}
