namespace GrpcKopikoDemoServer.ObjectModels
{
	public class RefreshTokenModel
	{
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}
