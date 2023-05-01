using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrpcKopikoDemoServer.DataModels
{
	public class Order
	{
        long _OrderID;

		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long OrderID { get { return _OrderID; } set { _OrderID = value; } }

		string _OrderNumber;
		public string OrderNumber { get { return _OrderNumber; } set { _OrderNumber = value; } }

		string _OrderShipName;
        public string OrderShipName { get { return _OrderShipName; } set { _OrderShipName = value; } }

        string _OrderShipAddress;
        public string OrderShipAddress { get { return _OrderShipAddress; } set { _OrderShipAddress = value; } }

        double _OrderTotalPrice;
        public double OrderTotalPrice { get { return _OrderTotalPrice; } set { _OrderTotalPrice = value; } }

        [ForeignKey(nameof(user))]
        public long UserID { get; set; }
        public User user { get; set; }

        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
