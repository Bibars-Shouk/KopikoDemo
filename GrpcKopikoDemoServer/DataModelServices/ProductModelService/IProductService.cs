using GrpcKopikoDemoServer.DataModels;

namespace GrpcKopikoDemoServer.DataModelServices.ProductModelService
{
	public interface IProductService
	{
		public Task<List<Product>> GetAll();		
	}
}
