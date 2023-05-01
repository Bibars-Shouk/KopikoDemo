using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrpcKopikoDemoServer.DataModels
{
	public class Product
	{
		long _ProductID;

		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long ProductID { get { return _ProductID; } set { _ProductID = value; } }

		string _ProductName;
		public string ProductName { get { return _ProductName; } set { _ProductName = value; } }

		string _ProductDescription;
		public string ProductDescription { get { return _ProductDescription; } set { _ProductDescription = value; } }

		string _ProductImageID;
		public string ProductImageID { get { return _ProductImageID; } set { _ProductImageID = value; } }

		double _Price;
		public double Price { get { return _Price; } set { _Price = value; } }

		public virtual ICollection<OrderDetails> OrderDetails { get; set; }

	}
}
