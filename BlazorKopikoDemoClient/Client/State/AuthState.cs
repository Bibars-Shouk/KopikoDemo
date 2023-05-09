using Blazored.LocalStorage;
using BlazorKopikoDemoClient.Client.DataModels;
using BlazorKopikoDemoClient.Protos;

namespace BlazorKopikoDemoClient.Client.State
{
    public class AuthState
    {
        private readonly ILocalStorageService _localstorage;
        public event Action OnChange;

        private AuthData _authData = new AuthData
        {
            IsAuthenticated = false,
            AccessToken = String.Empty,
            AccessTokenExpiryDate = DateTime.MinValue,
            RefreshToken = String.Empty,
            RefreshTokenExpiryDate = DateTime.MinValue,
        };

        public AuthData AuthData { get { return _authData; } set { _authData = value; OnChange?.Invoke(); } }

        public AuthState(ILocalStorageService localstorage)
        {
            _localstorage = localstorage;
        }

        public bool CheckRefreshTokenExpiryDate(DateTime refreshTokenExpiryDate)
        {
            if (refreshTokenExpiryDate.AddMinutes(-5) < DateTime.UtcNow) return false;
            else return true;
        }

        public bool CheckAccessTokenExpiryDate(DateTime accessTokenExpiryDate)
        {
            if (accessTokenExpiryDate < DateTime.UtcNow) return false;
            else return true;
        }

        public async Task LogoutUser()
        {
            AuthData = new AuthData
            {
                IsAuthenticated = false,
                AccessToken = String.Empty,
                AccessTokenExpiryDate = DateTime.MinValue,
                RefreshToken = String.Empty,
                RefreshTokenExpiryDate = DateTime.MinValue,
            };
            OnChange?.Invoke();

            await _localstorage.RemoveItemAsync("auth");          
        }

        public async Task InitUserAuth()
        {
            var isAuthExist = await _localstorage.ContainKeyAsync("auth");
            if (!isAuthExist)
            {
                await _localstorage.SetItemAsync<AuthData>("auth", AuthData);
            }
            else
            {
                var AuthDateTmp = await _localstorage.GetItemAsync<AuthData>("auth");

                if (AuthDateTmp.IsAuthenticated)
                {
                    if (!CheckRefreshTokenExpiryDate(AuthDateTmp.RefreshTokenExpiryDate))
                    {
                        await LogoutUser();
                    }
                    else if (!CheckAccessTokenExpiryDate(AuthDateTmp.AccessTokenExpiryDate))
                    {
                        // refresh access token
                    }
                    else
                    {
                        AuthData = AuthDateTmp;
                        OnChange?.Invoke();
                    }
                }
            }
        }

        // HandleAuthReply only handles changing the auth state and saving it to the localstorage
        public async Task HandleAuthReply(AuthReply reply)
        {
            AuthData = new AuthData
            {
                IsAuthenticated = true,
                AccessToken = reply.AccessToken,
                AccessTokenExpiryDate = reply.AccessTokenExpiryDate.ToDateTime(),
                RefreshToken = reply.RefreshToken,
                RefreshTokenExpiryDate = reply.RefreshTokenExpiryDate.ToDateTime(),
            };

            OnChange?.Invoke();

            await _localstorage.SetItemAsync<AuthData>("auth", AuthData);
        }
    }
}
