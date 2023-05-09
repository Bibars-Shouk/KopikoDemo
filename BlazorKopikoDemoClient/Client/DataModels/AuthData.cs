namespace BlazorKopikoDemoClient.Client.DataModels
{
    public class AuthData
    {
        public bool IsAuthenticated { get; set; }

        public string AccessToken { get; set; }

        public DateTime AccessTokenExpiryDate { get; set; }

        public string RefreshToken { get; set; }

        public DateTime RefreshTokenExpiryDate { get; set; }
    }
}
