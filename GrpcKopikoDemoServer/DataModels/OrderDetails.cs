using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcKopikoDemoServer.DataModels
{
	public class OrderDetails
	{
		long _OrderDetailsID;

		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long OrderDetailsID { get { return _OrderDetailsID; } set { _OrderDetailsID = value; } }

		string _OrderNumber;
		public string OrderNumber { get { return _OrderNumber; } set { _OrderNumber = value; } }

		int _Quantity;
		public int Quantity { get { return _Quantity; } set { _Quantity = value; } }

		double _PricePerPiece;
		public double PricePerPiece { get { return _PricePerPiece; } set { _PricePerPiece = value; } }

		double _TotalPrice;
		public double TotalPrice { get { return _TotalPrice; } set { _TotalPrice = value; } }

        [ForeignKey(nameof(product))]
		public long ProductID { get; set; }
		public Product product { get; set; }

		[ForeignKey(nameof(order))]
		public long OrderID { get; set; }
		public Order order { get; set; }

		[ForeignKey(nameof(user))]
		public long UserID { get; set; }
		public User user { get; set; }
	}
}
