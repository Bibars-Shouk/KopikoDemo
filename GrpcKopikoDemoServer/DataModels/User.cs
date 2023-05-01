using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GrpcKopikoDemoServer.DataModels
{
	public class User
	{
		long _UserID;

		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public long UserID { get { return _UserID; } set { _UserID = value; } }

		string _FirstName;
		public string FirstName { get { return _FirstName; } set { _FirstName = value; } }

		string _LastName;
		public string LastName { get { return _LastName; } set { _LastName = value; } }

		string _Email;
		public string Email { get { return _Email; } set { _Email = value; } }

		string _Password;
		public string Password { get { return _Password; } set { _Password = value; } }

        string _RefreshToken;
		public string RefreshToken { get { return _RefreshToken; } set { _RefreshToken = value; } }

		DateTime _RefreshTokenExpiryDate;
		public DateTime RefreshTokenExpiryDate { get { return _RefreshTokenExpiryDate; } set { _RefreshTokenExpiryDate = value; } }

		public virtual ICollection<Order> order { get; set; }
		public virtual ICollection<OrderDetails> orderDetails { get; set; }
	}
}
