using GrpcKopikoDemoServer.Data;
using GrpcKopikoDemoServer.DataModels;
using Microsoft.EntityFrameworkCore;

namespace GrpcKopikoDemoServer.DataModelServices.ProductModelService
{
	public class ProductService : IProductService
	{
		private readonly DatabaseContext _context;

		public ProductService(DatabaseContext context) 
		{
			_context = context;
		}

		async Task<List<Product>> IProductService.GetAll()
		{
			try
			{
				var products = await _context.Products.ToListAsync();
				return products;
			}
			catch (Exception)
			{
				throw;
			}
		}

		async Task<Product> IProductService.GetById(long productId)
		{
			try
			{
				var prodcut = await _context.Products.FirstOrDefaultAsync(x => x.ProductID == productId);
				return prodcut;
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
