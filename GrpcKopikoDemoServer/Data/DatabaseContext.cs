using GrpcKopikoDemoServer.DataModels;
using Microsoft.EntityFrameworkCore;

namespace GrpcKopikoDemoServer.Data
{
	public class DatabaseContext : DbContext
	{
		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
		public DbSet<User> Users { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<OrderDetails> OrderDetails { get; set; }
		public DbSet<Order> Orders { get; set; }
	}
}
